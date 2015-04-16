[![Build status](https://ci.appveyor.com/api/projects/status/xku8l63dxt24owk4/branch/master?svg=true)](https://ci.appveyor.com/project/taylorjg/flowfreesolverwpf/branch/master)

## Flow Free Solver

This project is a WPF version of my earlier [FlowFreeDlx](https://github.com/taylorjg/FlowFreeDlx "FlowFreeDlx") project.

## Screenshot

![Flow Free Solver Screenshot](https://raw.github.com/taylorjg/FlowFreeSolverWpf/master/Images/Screenshot.png "Flow Free Solver Screenshot")

## Development Steps

* ~~get basic layout with window, grid, controls area~~
* ~~draw fixed size grid~~
* ~~add grid size dropdown and draw dynamically sized grid~~
* ~~add ability to add dots to the grid~~
* ~~add ability to un-add a dot to the grid~~
* ~~pre-populate a 7x7 grid with a puzzle~~
* ~~add ability to draw a path (but without the highlighting of cells)~~
* ~~add a solve button~~
* ~~solve (on UI thread!) + draw solution paths~~
* ~~add highlight to paths~~
* ~~do solve on background thread~~
* ~~display modal dialog box with indeterminate progress bar during solving~~
* ~~add a cancel button to the modal dialog box~~
* ~~add ability to cancel the solve process~~
* ~~add status bar to display stats~~
* ~~add stats re number of rows/cols in the matrix~~
* ~~add stats re time taken to build the matrix~~
* ~~add stats re time taken for Dlx to solve the matrix~~
* ~~add indication of user cancellation (when it applies)~~
* ~~display stats so far even after cancellation ?~~
* ~~add ability to click on the BoardCanvas~~
* ~~raise an event that identifies the cell that was clicked (hit test ?)~~
* ~~add a dropdown with a list of colours, add dot of selected colour when clicking on the board~~
* ~~un-add a dot when clicking on an existing dot~~
* ~~display the colour in the dropdown items (use an item template ?)~~
* ~~add validation of colour pairs on the grid~~
    * ~~must be exact pairs~~
    * ~~minimum number of pairs for the selected grid size~~
    * ~~maximum number of pairs for the selected grid size~~
* ~~add a clear/reset button~~
* ~~pass maxDirectionChanges into BuildMatrixFor()~~
* ~~increase maxDirectionChanges (extending the matrix) until a solution is found~~
* add an app icon
* ~~enhance the validation messages (identify the exact problem)~~
    * ~~indicate minimum number of pairs if too few~~
    * ~~indicate maximum number of pairs if too many~~
    * ~~indicate which colours are not exact pairs~~
        * Now that I am using MVVM, the Solve button is not enabled unless the dots are valid
* ~~use TPL to try to speed up the creation of the matrix ?~~
* ~~Fix the bug introduced by using Parallel.ForEach()~~
* ~~Add an InternalDataRow inner class to simplify the MatrixBuilder code~~
* ~~apply the MVVM design pattern (MVVM Light Toolkit ?)~~

## Update (16th April 2015)

The solver works quickly for small grid sizes but starts to take a long time for larger grids.
It would be nice to improve the solve time for larger grids. Here are some ideas about how to
achieve this:

* Micro optimisations here and there (e.g. inside loops that occur thousands of times)

* When finding paths, favour ones that maintain direction e.g. if heading left, we currently explore left, up and down. We should probably only explore left initially. We can explore up and down later if necessary.

* The majority of the time is spent finding paths i.e. building the Dlx matrix. The Dlx solve time is very quick. At the moment, we only attempt a Dlx solve after building the martrix for a given max number of direction changes. Perhaps we could attempt Dlx solves as the matrix is being built. We could potentially use
[TPL Dataflow](https://msdn.microsoft.com/en-us/library/hh228603%28v=vs.110%29.aspx)
to organise this e.g. using a BatchBlock.
