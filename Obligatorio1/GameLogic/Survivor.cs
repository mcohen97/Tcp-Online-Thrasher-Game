﻿using System;

namespace GameLogic
{
    public class Survivor:Player
    {
        public static readonly int DEFAULT_SURVIVOR_HEALTH = 20;
        private Role role;
        private int health;
        private Position position;

        public Survivor()
        {
            this.health = DEFAULT_SURVIVOR_HEALTH;
            base.attackTechnique = new SurvivorAttackTechnique();
            this.role = Role.SURVIVOR;
            this.position = new Position(0, 0);
        }

        public Survivor(Position initialPosition)
        {
            this.health = DEFAULT_SURVIVOR_HEALTH;
            base.attackTechnique = new SurvivorAttackTechnique();
            this.role = Role.SURVIVOR;
            this.position = initialPosition;
        }

        public override int Health {
            get {
                return this.health;
            }

            protected set {
                health = value;
            }
        }

        public override Role Role {
            get {
                return this.role;
            }

            protected set {               
            }
        }

        public override Position Position {
            get {
                return this.position;
            }

            set {
                position = value;
            }
        }
    }
}