﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Polymino
    {
        const double rotationAngle = Math.PI/2;
        public Polymino(Types type)
        {
            Type = type;
        }

        public Polymino(Types type, List<Point> points)
        {
            Type = type;
            Points = points;
        }

        public Types Type { get; set; }

        private List<Point> points;

        public List<Point> Points
        {
            get => points == null || !points.Any() ? Pentominos.pentominos[Type] : points;
            set => points = new List<Point>(value);
        }

        public List<Point> CanPlaceInEmptyRectangle(int width, int height)
        {
            Board board = new Board(width, height);
            List<Point> result = new List<Point>();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (board.CanPolyminoBePlacedInEmpty(j, i, this))
                    {
                        result.Add(new Point(i, j));
                    }
                }
            }
            return result;
        }

        public Polymino Rotate(double angle)
        {
            int cos = (int)Math.Cos(angle);
            int sin = (int)Math.Sin(angle);
            var rotatedPoints = Points.Select(p => new Point((int)(p.X * cos - p.Y * sin),(int)( p.X * sin+ p.Y * cos))).ToList();
            Solver.AdjustPolyminoPoints(rotatedPoints);

            return new Polymino(this.Type, rotatedPoints);
        }

        public List<Polymino> Rotations()
        {
            List<Polymino> rotatedPolyminos = new List<Polymino>();
            for (double angle = rotationAngle; angle <= 4*rotationAngle; angle += rotationAngle)
            {
                rotatedPolyminos.Add(Rotate(angle));
            }

            //TODO: Tomek sprawdz

            return GetDistinctRotations(rotatedPolyminos);
        }

        private static List<Polymino> GetDistinctRotations(List<Polymino> polyminos)
        {
            var result = new List<Polymino>();
            foreach (var p in polyminos)
            {
                p.SortPoints();
            }

            polyminos.ForEach(p =>
            {
                if (!result.Any(r => PointsAreEqual(p, r)))
                {
                    result.Add(p);
                }
            });

            return result;
        }

        private void SortPoints()
        {
            Points.Sort((p1, p2) =>
            {
                if (p1.X < p2.X)
                    return -1;
                if (p1.X == p2.X && p1.Y < p2.Y)
                    return -1;
                return 1;
            });
        }

        private static bool PointsAreEqual(Polymino p1, Polymino p2)
        {
            return p1.points.Count == p2.points.Count
                   && !p1.points.Where((t, i) => t.X != p2.points[i].X || t.Y != p2.points[i].Y).Any();
        }
    }
}
