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

        private string? _originalName;
        

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

        [RelayCommand]
        public void UpdateCollection()
        {
            while (HasMultipleCollectionsWithName(EditableCollection.Name))
            {
                EditableCollection.Name += "1";
            }

            WordCollection collectionToUpdate = GetCollectionByName(EditableCollection.Name);

            if (collectionToUpdate != null)
            {
                // Collection retains the name
                collectionToUpdate = EditableCollection;
            }
            else
            {
                // Name changed
                Collections.Add(EditableCollection);
                collectionToUpdate = Collections.Last();
            }

            _collectionSerice.SaveCollection(collectionToUpdate);

            if (_originalName != null && _originalName != EditableCollection.Name)
            {
                // The name has changed. Remove old collection file
                _collectionSerice.DeleteCollection(_originalName);
                int collectionIndexToRemove = GetCollectionIndexByName(_originalName);

                if (collectionIndexToRemove > -1 && collectionIndexToRemove < Collections.Count)
                {
                    Collections.RemoveAt(collectionIndexToRemove);
                }
            }
        }

        [RelayCommand]
        public void RemoveCollection()
        {
            if (SelectedCollection.Name != null)
            {
                _collectionSerice.DeleteCollection(SelectedCollection.Name);
                int collectionIndexToRemove = GetCollectionIndexByName(SelectedCollection.Name);

                if (collectionIndexToRemove > -1 && collectionIndexToRemove < Collections.Count)
                {
                    Collections.RemoveAt(collectionIndexToRemove);
                }

            }
        }

        private bool HasMultipleCollectionsWithName(string name)
        {
            return Collections.Count(c =>
                string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)) > 1;
        }

        private WordCollection? GetCollectionByName(string name)
        {
            return Collections.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        private int GetCollectionIndexByName(string name)
        {
            return Collections.ToList().FindIndex(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        partial void OnSelectedCollectionChanged(WordCollection? value)
        {
            if (value != null)
            {
                _originalName = value.Name;
                EditableCollection = new WordCollection(value.Name, value.Words);
            }
            else
            {
                _originalName = null;
                EditableCollection = null;
            }
        }
    }
}
