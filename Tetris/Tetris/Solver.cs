﻿using System;
using System.Collections.Generic;

namespace Tetris {
	public enum ProblemType
	{
		Square,
		Rectangle
	}

	public enum AlgorithmType
	{
		Precise,
		Heuristic
	}

	public static class Solver
	{
        const int polyminoSize = 5;
        public static List<Board> Solve(ProblemType problemType, AlgorithmType algorithmType, List<Polymino> polyminos)
        {

            if (problemType == ProblemType.Square)
            {
                if (algorithmType == AlgorithmType.Precise)
                {
					return PreciseSquareSolver.Solve(polyminos);
                }
                else
                {
                    return HeuristicSquareSolver.Solve(polyminos);
                }
            }
            else
            {
                if (algorithmType == AlgorithmType.Precise)
                {
					return PreciseRectangleSolver.Solve(polyminos);
                }
                else
                {
					return HeuristicRectangleSolver.Solve(polyminos);
                }
            }
        }

        public static int CalculateMinimalSquare(List<Polymino> pentominos)
        {
            return (int)Math.Ceiling(Math.Sqrt(polyminoSize * pentominos.Count));
        }

        public static (int, int) CalculateMinimalRectangle(List<Polymino> pentominos)
        {
            int rectangleArea = polyminoSize * pentominos.Count;
            int potentialSide = (int)Math.Sqrt(rectangleArea);
            while (rectangleArea % potentialSide != 0 && potentialSide > 0)
            {
                potentialSide--;
            }
            if (potentialSide == 0)
                throw new ArgumentException("cannot fit pentominos");

            return (rectangleArea / potentialSide, potentialSide);
        }

        // Ta funkcja działa tylko dla pentomino
        public static Dictionary<Types, List<Point>> PotentiallyValidPositions(List<Polymino> polyminos, int width, int height)
        {
            var result = new Dictionary<Types, List<Point>>();
            foreach (var polymino in polyminos)
            {
                if (!result.ContainsKey(polymino.Type))
                    result[polymino.Type] = polymino.CanPlaceInEmptyRectangle(width, height);
            }
            return result;
        }
    }
}