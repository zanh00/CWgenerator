using CommunityToolkit.Mvvm.ComponentModel;
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
        private WordCollection selectedCollection;

        public EditCollectionsViewModel(List<WordCollection> collections)
        {
            _collectionSerice = new WordCollectionService();
            _collections = collections;
        }
    }
}
