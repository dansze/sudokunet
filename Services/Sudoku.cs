using System;

namespace sudokunet.Services {

    public class SudokuService {

        private int[,] currentSudoku = {
             {1, 2, 3, 4, 5, 6, 7, 8, 9},
             {4, 5, 6, 7, 8, 9, 1, 2, 3},
             {7, 8, 9, 1, 2, 3, 4, 5, 6},
             {2, 3, 1, 5, 6, 4, 8, 9, 7},
             {5, 6, 4, 8, 9, 7, 2, 3, 1},
             {8, 9, 7, 2, 3, 1, 5, 6, 4},
             {3, 1, 2, 6, 4, 5, 9, 7, 8},
             {6, 4, 5, 9, 7, 8, 3, 1, 2},
             {9, 7, 8, 3, 1, 2, 6, 4, 5}
         };

        private Random random;

        public SudokuService() {
            random = new Random();
        }

        private int[,] pairs = {
            {0, 1},
            {0, 2},
            {1, 0},
            {1, 2},
            {2, 0},
            {2, 1},
        };
        public int[,] GetNextPuzzle() {
            for (int i = 0; i < 27; i++) { //8 is a local minimum in automorphic sudoku, minimizing repeats
                int pair = random.Next(0, 6);
                int op = random.Next(0, 4);
                switch(op) {
                    case 0:
                        swapRows(currentSudoku, pairs[pair,0], pairs[pair,1]);
                        break;
                    case 1:
                        swapCols(currentSudoku, pairs[pair,0], pairs[pair,1]);
                        break;
                    case 2:
                        swapBands(currentSudoku, pairs[pair,0], pairs[pair,1]);
                        break;
                    default:
                        swapStacks(currentSudoku, pairs[pair,0], pairs[pair,1]);
                        break;
                }
            }
            return (int[,])currentSudoku.Clone();
        }

        public int[,] MinimizePuzzle(int[,] solution) {
            int [,] puzzle = (int[,])solution.Clone();

            //Instead of randomizing an array of 1 through 9, we can just use two random rows
            //to randomize access order
            int r1 = random.Next(0,9);
            int r2 = random.Next(0,9);

            /*
             Each element need only be checked once. Either removing it would make the puzzle
             ambiguous, which will continue to be true if other elements are removed, or it can
             be safely removed.
             */
            for (int c1 = 0; c1 < 9; c1++) {
                for (int c2 = 0; c2 < 9; c2++) {
                    bool[] found = new bool[9];
                    int row = solution[r1,c1] - 1;                //guaranteed non-0
                    int col = solution[r2,c2] - 1;
                    for (int r = 0; r < 9; r++) {
                        if(puzzle[r, col] > 0)
                            found[puzzle[r, col] - 1] = true;
                    }
                    for (int c = 0; c < 9; c++) {
                        if(puzzle[row, c] > 0)
                            found[puzzle[row, c] - 1] = true;
                    }
                    int squareRow = row - row%3;                               //Move to the corner of the square
                    int squareCol = col - col%3;
                    for (int i = 0; i < 9; i++) {
                        if(puzzle[squareRow + i/3, squareCol + i%3] > 0)
                            found[puzzle[squareRow + i/3, squareCol + i%3] - 1] = true;
                    }

                    if (Array.TrueForAll(found, b => b))
                        puzzle[row,col] = 0;
                }
            }

            return puzzle;
        }

        private int[,] swapRows(int[,] puzzle, int first, int second) {
            return swapRegions(puzzle, 1, 9, first, 0, second, 0);
        }

        private int[,] swapCols(int[,] puzzle, int first, int second) {
            return swapRegions(puzzle, 9, 1, 0, first, 0, second);
        }

        private int[,] swapBands(int[,] puzzle, int first, int second) {
            return swapRegions(puzzle, 3, 9, first * 3, 0, second * 3, 0);
        }

        private int[,] swapStacks(int[,] puzzle, int first, int second) {
            return swapRegions(puzzle, 9, 3, 0, first * 3, 0, second * 3);
        }

        //Swap some rows in the given puzzle in place.
        //if col is true, swap columns instead.
        //returns the puzzle for chaining
        private int[,] swapRegions(int[,] puzzle, int rows, int cols, int firstRow, int firstCol, int secondRow, int secondCol) {

            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {
                    int temp = puzzle[firstRow + r, firstCol + c];
                    puzzle[firstRow + r, firstCol + c] = puzzle[secondRow + r, secondCol + c];
                    puzzle[secondRow + r, secondCol + c] = temp;
                }
            }
            return puzzle;
        }
    }
    
}