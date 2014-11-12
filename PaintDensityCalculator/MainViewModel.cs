﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using PaintDensityCalculator.Models;

namespace PaintDensityCalculator
{
    public class MainViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        public MainViewModel()
        {
            //Добавление типов плотности для первого комбобокса с типами
            DenTypes = new ObservableCollection<DensityType>
                {
                    new DensityType {Id = (int) DensityTypes.Mobihel, Name = "Mobihel"},
                    new DensityType {Id = (int) DensityTypes.Grunts, Name = "Грунты"},
                    new DensityType {Id = (int) DensityTypes.Thinner, Name = "Разбавитель"},
                    new DensityType {Id = (int) DensityTypes.Hardener, Name = "Отвердитель"},
                    new DensityType {Id = (int) DensityTypes.Others, Name = "Прочее"}
                };

            //Достать все из таблицы плотностей чтобы забиндить на 2й комбобокс
            Densities = new ObservableCollection<Density>(colorItEntities.Density.ToList());

            GruntDensities =
                new ObservableCollection<Density>(colorItEntities.Density.
                    Where(gr => gr.Type == (int)DensityTypes.Grunts).ToList());


            _densitiesComboView = CollectionViewSource.GetDefaultView(Densities);
            _densitiesComboView.Filter = DensityFilter;

            _gruntDensitiesComboView = CollectionViewSource.GetDefaultView(GruntDensities);
            _gruntDensitiesComboView.Filter = GruntDensityFilter;

            AddCommands();

        }

        #region IDataErrorInfo Members

        private int _errors = 0;
        public void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case "LeftProportion":
                        if (CurrentGruntMixDensity != null && LeftProportion == 0)
                            result = "Пропорция не может быть равна нулю";
                        break;
                    case "RightProportion":
                        if (CurrentGruntMixDensity != null && RightProportion == 0)
                            result = "Пропорция не может быть равна нулю";
                        break;
                }
                return result;
            }
        }

        #endregion

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties

        private DensityType _currentType;
        private Density _currentDensity;
        private Density _currentGruntMixDensity;
        private double _volume;
        private double _mass;
        private double _volHardener;
        private double _volThinner;
        private double _massHardener;
        private double _massThinner;
        private int _leftProportion; 
        private int _rightProportion;
        private ICollectionView _densitiesComboView;
        private ICollectionView _gruntDensitiesComboView;
        private string _leftGruntMass;
        private string _rightGruntMass;

        public ObservableCollection<DensityType> DenTypes { get; set; }
        public ObservableCollection<Density> Densities { get; set; }
        public ObservableCollection<Density> GruntDensities { get; set; }

        public DensityType CurrentType
        {
            get { return _currentType; }
            set
            {
                _currentType = value;
                OnPropertyChanged("CurrentType");
                _densitiesComboView.Refresh();
            }
        }

        public Density CurrentDensity
        {
            get { return _currentDensity; }
            set
            {
                _currentDensity = value;
                OnPropertyChanged("CurrentDensity");
                _gruntDensitiesComboView.Refresh();
            }
        }

        public Density CurrentGruntMixDensity
        {
            get { return _currentGruntMixDensity; }
            set
            {
                _currentGruntMixDensity = value;
                OnPropertyChanged("CurrentGruntMixDensity");
                LeftProportion = 0;
                RightProportion = 0;
            }
        }

        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                OnPropertyChanged("Volume");
                TotalRecalc();
            }
        }

        public double Mass
        {
            get { return _mass; }
            set
            {
                _mass = value;
                OnPropertyChanged("Mass");
            }
        }

        public double VolHardener
        {
            get { return _volHardener; }
            set
            {
                _volHardener = value;
                OnPropertyChanged("VolHardener");
            }
        }

        public double VolThinner
        {
            get { return _volThinner; }
            set
            {
                _volThinner = value;
                OnPropertyChanged("VolThinner");
            }
        }

        public double MassHardener
        {
            get { return _massHardener; }
            set
            {
                _massHardener = value;
                OnPropertyChanged("MassHardener");
            }
        }

        public double MassThinner
        {
            get { return _massThinner; }
            set
            {
                _massThinner = value;
                OnPropertyChanged("MassThinner");
            }
        }

        public int LeftProportion
        {
            get { return _leftProportion; }
            set
            {
                _leftProportion = value;
                OnPropertyChanged("LeftProportion");
            }
        }

        public int RightProportion
        {
            get { return _rightProportion; }
            set
            {
                _rightProportion = value;
                OnPropertyChanged("RightProportion");
            }
        }

        public ICollectionView DensitiesComboView
        {
            get { return _densitiesComboView; }
        }

        public ICollectionView GruntDensitiesComboView
        {
            get { return _gruntDensitiesComboView; }
        }

        public string LeftGruntMass
        {
            get { return _leftGruntMass; }
            set
            {
                _leftGruntMass = value;
                OnPropertyChanged("LeftGruntMass");
            }
        }

        public string RightGruntMass
        {
            get { return _rightGruntMass; }
            set
            {
                _rightGruntMass = value;
                OnPropertyChanged("RightGruntMass");
            }
        }

        #endregion

        #region Methods


        private bool DensityFilter(object item)
        {
            if (_currentType == null)
                return true;

            Density density = item as Density;
            return density != null && density.Type == _currentType.Id;
        }

        //Жестокий фильтр для грунтов предназначеных для смешивания..
        private bool GruntDensityFilter(object item)
        {
            Density density = item as Density;

            if (CurrentDensity == null || density == null)
                return false;

            int[] FillerWet = { 34, 35, 36 };
            int[] FillerDry = { 51, 52, 53 };
            int[] FillerHB = { 37, 38, 39 };
            int[] compareWith = { CurrentDensity.ID };

            if (FillerWet.Contains(CurrentDensity.ID))
                return FillerWet.Except(compareWith).Contains(density.ID);
            if (FillerDry.Contains(CurrentDensity.ID))
                return FillerDry.Except(compareWith).Contains(density.ID);
            if (FillerHB.Contains(CurrentDensity.ID))
                return FillerHB.Except(compareWith).Contains(density.ID);

            return false;
        }

        //Главный метод который обновляет все поля для расчета
        private void TotalRecalc()
        {
            if (CurrentGruntMixDensity == null)
                PaintMassCounter();
            else
                GruntMassCounter();

            UpdateVolThinner();
            UpdateMassThinner();

            UpdateVolHardener();
            UpdateMassHardener();
        }

        //Высчитывает массу краски\прочего, не грунта
        private void PaintMassCounter()
        {
            Mass = CurrentDensity.DensityValue * Volume * 1000;
        }

        //Высчитывает массу грунта c пропорциями
        private void GruntMassCounter()
        {

            double onePart = Volume / (LeftProportion + RightProportion);
            double leftGrunt = Math.Round((onePart * LeftProportion * CurrentDensity.DensityValue * 1000), 1);
            double rightGrunt = Math.Round((onePart * RightProportion * CurrentGruntMixDensity.DensityValue * 1000), 1);
            Mass = leftGrunt + rightGrunt;

            LeftGruntMass = String.Format("Масса {0} = {1}", CurrentDensity.Name, leftGrunt);
            RightGruntMass = String.Format("Масса {0} = {1}", CurrentGruntMixDensity.Name, rightGrunt);
        }

        //Считает объем разбавителя
        private void UpdateVolThinner()
        {
            VolThinner = Volume * CurrentDensity.ProportionThinner;
        }

        //Считает массу разбавителя
        private void UpdateMassThinner()
        {
            double thinnerValue = 0;
            var firstOrDefault = colorItEntities.Density.FirstOrDefault(density => density.ID == CurrentDensity.AccordingThinner);
            if (firstOrDefault != null)
            {
                thinnerValue = firstOrDefault.DensityValue;
            }

            MassThinner = thinnerValue * VolThinner * 1000;
        }

        //Считает объем отвердителя
        private void UpdateVolHardener()
        {
            VolHardener = Volume * CurrentDensity.ProportionHardener;
        }

        //Считает массу отвердителя
        private void UpdateMassHardener()
        {
            double hardenerValue = 0;
            var firstOrDefault = colorItEntities.Density.FirstOrDefault(density => density.ID == CurrentDensity.AccordingHardener);
            if (firstOrDefault != null)
            {
                hardenerValue = firstOrDefault.DensityValue;
            }

            MassHardener = hardenerValue * VolHardener * 1000;
        }

        //Очищает все поля
        private void ClearAllFields()
        {
            CurrentGruntMixDensity = null;
            Volume = 0;
            LeftGruntMass = String.Empty;
            RightGruntMass = String.Empty;
            LeftProportion = 0;
            RightProportion = 0;
        }

        #endregion

        #region Commands

        public RelayCommand CleanAllCommand
        {
            get;
            private set;
        }

        //Initialize commands
        private void AddCommands()
        {
            CleanAllCommand = new RelayCommand(ClearAllFields);
        }
        

        #endregion

        public enum DensityTypes
        {
            Mobihel = 0,
            Grunts = 1,
            Others = 2,
            Thinner = 3,
            Hardener = 4
        }
    }
}
