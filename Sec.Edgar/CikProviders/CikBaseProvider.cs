using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sec.Edgar.Models.Edgar;
using Sec.Edgar.Models.Exceptions;

namespace Sec.Edgar.CikProviders
{
    internal abstract class CikBaseProvider : ICikProvider
    {
        internal readonly ModelManager CikDataManager;
        internal readonly ILogger Logger;

        protected CikBaseProvider(ILogger logger, int cikIdentifierLength, bool fillCikIdentifierWithZeroes,
            CancellationToken ctx)
        {
            CikDataManager = new ModelManager(cikIdentifierLength, fillCikIdentifierWithZeroes);
            CikDataManager.ExceptionHandler += LogException;
            Ctx = ctx;
            Logger = logger;
        }

        protected CancellationToken Ctx { get; private set; }

        public virtual Task UpdateCikDataset()
        {
            throw new NotImplementedException();
        }

        public virtual Task<string> GetFirstAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string> GetFirstAsync(int cikNumber)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<EdgarTickerJsonDto>> GetAllAsync(int cikNumber)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<EdgarTickerJsonDto>> GetAllAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        internal void LogException(object sender, ExceptionEventArgs e)
        {
            Logger?.Log(e.LoggedLevel, $"{DateTime.Now:u}. Sender: {sender}. Exception: {e.LoggedException}");
            if (e.ReThrow)
            {
                throw e.LoggedException;
            }
        }

        protected SourceType GetSourceType(string absoluteSourceLocation)
        {
            if (Uri.IsWellFormedUriString(absoluteSourceLocation, UriKind.Absolute))
            {
                return SourceType.Web;
            }

            return File.Exists(absoluteSourceLocation)
                ? SourceType.Local
                : SourceType.None;
        }
    }
}