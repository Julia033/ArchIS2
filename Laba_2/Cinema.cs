using System;

namespace Laba_2
{
    class Cinema
    {
        private string? film;
        public string Film { get { return film; } set { film = value; } }
        private string? datetime;
        public string DateTime { get { return datetime; } set { datetime = value; } }
        private bool available_seats;
        public bool Available_seats { get { return available_seats; } set { available_seats = value; } }
        private int total_seats;
        public int Total_seats
        {
            get { return total_seats; }
            set
            {
                if (value < 0)
                    total_seats = Math.Abs(value);
                else
                    total_seats = value;
            }
        }

    }
}
