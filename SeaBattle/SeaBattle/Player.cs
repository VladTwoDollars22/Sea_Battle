﻿namespace SeaBattle
{
    public class Player
    {
        public bool isBot;
        public Field field;
        public List<int> ships;
        public int HP;

        public Player(bool isBot = false)
        {
            isBot = isBot;

            field = new Field(9, 9);

            ships = new List<int> { 4, 3 , 3, 2, 2, 2, 1, 1, 1, 1 };

            HP = ships.Sum();
        }
    }
}