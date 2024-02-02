using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace testApp.Models
{
    public class UserInfo
    {
        private string user { get; set; }
        private string positionUser { get; set; }
        private string chairman { get; set; }
        private string positionChairman { get; set; }
        private string commissionMember1 { get; set; }
        private string positionCommissionMember1 { get; set; }
        private int numberMistake { get; set; }

        public string User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        public string PositionUser
        {
            get { return positionUser; }
            set
            {
                positionUser = value;
                OnPropertyChanged("PositionUser");
            }
        }

        public string Chairman
        {
            get { return chairman; }
            set
            {
                chairman = value;
                OnPropertyChanged("Chairman");
            }
        }

        public string PositionChairman
        {
            get { return positionChairman; }
            set
            {
                positionChairman = value;
                OnPropertyChanged("PositionChairman");
            }
        }
        public string CommissionMember1
        {
            get { return commissionMember1; }
            set
            {
                commissionMember1 = value;
                OnPropertyChanged("CommissionMember1");
            }
        }

        public string PositionCommissionMember1
        {
            get { return positionCommissionMember1; }
            set
            {
                positionCommissionMember1 = value;
                OnPropertyChanged("PositionCommissionMember1");
            }
        }
        public int NumberMistake
        {
            get { return numberMistake; }
            set
            {
                numberMistake = value;
                OnPropertyChanged("NumberMistake");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
