using CustomerManagerApp.Backend.Service;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CustomerManagerApp.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		private readonly DataService dataService;

		public MainWindowViewModel(DataService DataService)
		{
			this.dataService = DataService;

			ListViewModel = new(DataService);
			EditViewModel = new(DataService);
		}

		//Child View Models
		public ListViewModel ListViewModel { get; set; }
		public EditViewModel EditViewModel { get; set; }



		public bool IsLoading { get; private set; }  = false;
		public void Load()
		{
			//stops multiple refreshes firing at once.
			if (IsLoading) return;
			IsLoading = true;

            try
            {
				ListViewModel.Load();
				EditViewModel.Load();
			}
			catch(NotImplementedException ex)
            {
				Debug.Print($"{ex.GetType} exception recorded: On {ex.Source}, {ex.TargetSite} isn't supported yet.");
            }

			IsLoading = false;
		}
	}
}
