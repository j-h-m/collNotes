using collNotes.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace collNotes.Services
{
    public interface IExceptionRecordService
    {
        Task<bool> CreateExceptionRecord(Exception exception);

        Task<IEnumerable<ExceptionRecord>> GetAllAsync();

        Task<bool> AddAsync(ExceptionRecord exceptionRecord);
    }
}