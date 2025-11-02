using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrosswordsPuzzleGenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordsPuzzleGenerator.ViewModels
{
    partial class EditCollectionsViewModel : ObservableObject
    {
        private WordCollectionService _collectionSerice;

        [ObservableProperty]
        private ObservableCollection<WordCollection> _collections;

        [ObservableProperty]
        private WordCollection _selectedCollection;

        [ObservableProperty]
        private WordCollection _editableCollection;
        

        public EditCollectionsViewModel(ObservableCollection<WordCollection> collections)
        {
            _collectionSerice = new WordCollectionService();
            Collections = collections;

            if (collections.Count <= 0)
            {
                // We wan't to have at least one colleciton.
                AddNewCollection();
            }
            
        }

        [RelayCommand]
        public void AddNewCollection()
        {
            Collections.Add(new WordCollection("New collection"));
        }

        partial void OnSelectedCollectionChanged(WordCollection? value)
        {
            if (value != null)
            {

                EditableCollection = new WordCollection(value.Name, value.Words);
            }
            else
            {
                EditableCollection = null;
            }
        }
    }
}
