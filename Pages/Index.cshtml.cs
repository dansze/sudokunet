using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using sudokunet.Services;

namespace sudokunet.Pages
{
    public class IndexModel : PageModel
    {
        private SudokuService sudoku;

        public int[,] Puzzle;

        public int[,] Solution;

        public IndexModel(SudokuService sudokuService)
        {
            sudoku = sudokuService;
        }

        public void OnGet()
        {
            Solution = sudoku.GetNextPuzzle();
            Puzzle = sudoku.MinimizePuzzle(Solution);
        }
    }
}
