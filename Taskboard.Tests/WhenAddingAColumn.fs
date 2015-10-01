module ``When adding a column``

open Xunit
open Swensen.Unquote

[<Fact>]
let ``A column is added to the board when the column state exists`` () =
    Given [ 
            BoardCreated { Id = BoardId 1; Name = "Board"; }
            StateCreated { Id = StateId 1; Name = "State"; }
        ] 
    |> When (CreateColumnCommand { Board = BoardId 1; Name = "Column name";  State = StateId 1})
    |> Then [ ColumnCreated { Name = "Column name"; Id = ColumnId 0; State = StateId 1 } ]

[<Fact>]
let ``Another column is added to the board when the column states exists`` () =
    Given [ 
        BoardCreated { Id = BoardId 1; Name = "Board"; }
        StateCreated { Id = StateId 1; Name = "State 1"; }
        StateCreated { Id = StateId 2; Name = "State 2"; }
        StateCreated { Id = StateId 3; Name = "State 3"; }
        ColumnCreated { Name = "Column name"; Id = ColumnId 0; State = StateId 0 }  
        ColumnCreated { Name = "Column name"; Id = ColumnId 1; State = StateId 1 } ]
    |> When (CreateColumnCommand { Board = BoardId 1; Name = "New column"; State = StateId 2 } )
    |> Then [ ColumnCreated { Name = "New column"; Id = ColumnId 2; State = StateId 2 } ]

[<Fact>]
let ``Adding column fails when the state does not exist`` () =
    Given [ BoardCreated { Id = BoardId 1; Name = "Board"; } ]
    |> When (CreateColumnCommand { Board = BoardId 1; Name = "New column"; State = StateId 2 } )
    |> ThenThrows
