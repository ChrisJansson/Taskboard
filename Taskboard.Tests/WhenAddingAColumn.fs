module ``When adding a column``

open Xunit
open Swensen.Unquote

[<Fact>]
let ``A column is added to the board`` () =
    Given [ BoardCreated { Id = BoardId 1; Name = "Board"; }] 
    |> When (CreateColumnCommand { Board = BoardId 1; Name = "Column name" })
    |> Then [ ColumnCreated { Name = "Column name"; Id = ColumnId 0 } ]

[<Fact>]
let ``Another column is added to the board`` () =
    Given [ 
        BoardCreated { Id = BoardId 1; Name = "Board"; }
        ColumnCreated { Name = "Column name"; Id = ColumnId 0 }  
        ColumnCreated { Name = "Column name"; Id = ColumnId 1 } ]
    |> When (CreateColumnCommand { Board = BoardId 1; Name = "New column" } )
    |> Then [ ColumnCreated { Name = "New column"; Id = ColumnId 2 } ]
