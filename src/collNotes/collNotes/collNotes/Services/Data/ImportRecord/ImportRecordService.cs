using collNotes.Domain.Models;
using collNotes.Ef.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace collNotes.Services.Data
{
    public class ImportRecordService : IImportRecordService
    {
        private readonly CollNotesContext Context =
            DependencyService.Get<CollNotesContext>(DependencyFetchTarget.GlobalInstance);

        public async Task<bool> AddAsync(ImportRecord importRecord)
        {
            await Context.ImportRecords.AddAsync(importRecord);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> HasFileBeenImported(string fileName)
        {
            return await Context.ImportRecords.FirstOrDefaultAsync(x => x.FileName.Equals(fileName)) is ImportRecord;
        }

        public bool DeleteAll()
        {
            if (Context.ImportRecords.Any())
            {
                Context.ImportRecords.RemoveRange(Context.ImportRecords);
                return Context.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
