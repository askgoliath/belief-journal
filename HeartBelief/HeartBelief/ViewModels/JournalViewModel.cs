using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using HeartBelief.Models;
using Xamarin.Forms;

namespace HeartBelief.ViewModels
{
    public class JournalViewModel : BaseViewModel
    {
        public ObservableCollection<Models.Entry> Entries { get; set; }
        public ICommand LoadItemsCommand { get; set; }
        
        public ICommand EntryCompletedCommand { get; set; }

        public ICommand DeleteEntryCommand { get; set; }

        public JournalViewModel()
        {
            Title = "Journal";
            Entries = new ObservableCollection<Models.Entry>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            EntryCompletedCommand = new Command<Models.Entry>(
                async (Models.Entry entry) => 
                await DelayedQueryForKeyboardTypingSearches(entry)
            );

            DeleteEntryCommand = new Command<Models.Entry>(
              async (Models.Entry entry) =>
              await DeleteEntry(entry)
          );
        }

        async Task DeleteEntry(Models.Entry entry)
        {
            try
            {
                await EntriesStore.DeleteItemAsync(entry);
                Entries.Remove(entry);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task AddNewEntry()
        {
            var entry = new Models.Entry();
            int affected = await EntriesStore.SaveItemAsync(entry);
            if (affected > 0) Entries.Add(entry);
        }

        async Task SaveEntryCommand(Models.Entry entry)
        {
            if (entry == null) return;

            try
            {
                await EntriesStore.SaveItemAsync(entry);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private CancellationTokenSource throttleCts = new CancellationTokenSource();
        /// <summary>
        /// Runs in a background thread, checks for new Query and runs current one
        /// </summary>
        private async Task DelayedQueryForKeyboardTypingSearches(Models.Entry entry)
        {
            try
            {
                Interlocked.Exchange(ref throttleCts, new CancellationTokenSource()).Cancel();

                await Task.Delay(TimeSpan.FromMilliseconds(1500), throttleCts.Token)
                .ContinueWith(
                            async task => 
                            await SaveEntryCommand(entry),
                            CancellationToken.None,
                            TaskContinuationOptions.OnlyOnRanToCompletion,
                            TaskScheduler.FromCurrentSynchronizationContext()
                );
            }
            catch
            {
                //Ignore any Threading errors
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Entries.Clear();
                var entries = await EntriesStore.GetItemsAsync();
                foreach (var entry in entries)
                {
                    Entries.Add(entry);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
