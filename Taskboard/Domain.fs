[<AutoOpen>]
module Domain


type CardId = CardId of int

type Card = 
    {
        Id : CardId
    }

type ColumnId = ColumnId of int

type Column = 
    {
        Id : ColumnId
        Name : string
        Cards : Card list
    }
    
type BoardId = BoardId of int

type Board =
    {
        Id : BoardId
        Name : string
        Columns : Column list
        Created : bool
    }
    with 
    static member initial = { Id = BoardId 0; Name = "None"; Columns = []; Created = false }

    
type Command =
    | CreateBoardCommand of CreateBoardCommand
    | CreateColumnCommand of CreateColumnCommand
    | MoveColumnCommand of MoveColumnCommand

and CreateBoardCommand = 
    {
        Id : BoardId
        Name : string
    }

and CreateColumnCommand =
    {
        Board : BoardId
        Name : string
    }

and MoveColumnCommand = 
    {
        Id : ColumnId
        TargetPosition : int
    }

type Event =
    | BoardCreated of BoardCreated
    | ColumnCreated of ColumnCreated
    | ColumnMoved of ColumnMoved

and BoardCreated =
    {
        Id : BoardId
        Name : string
    }

and ColumnCreated =
    {
        Id : ColumnId
        Name : string
    }

and ColumnMoved =
    {
        Id : ColumnId
        Position : int
    }

let createBoard (command:CreateBoardCommand) state =
    if not state.Created then
        [ BoardCreated { Id = command.Id; Name = command.Name } ]
    else failwith "The board is already created"

let createColumn (command:CreateColumnCommand) state =
    [ ColumnCreated { Name = command.Name; Id = ColumnId state.Columns.Length } ]

let containsColumn board columnId =
    board.Columns |> List.map (fun c -> c.Id) |> List.contains columnId

let moveColumn (command:MoveColumnCommand) state =
    if command.TargetPosition < 0 then
        failwith "Invalid position"
    let numberOfColumns = state.Columns.Length
    if command.TargetPosition > (numberOfColumns - 1) then
        failwith "Invalid position"
    if not (containsColumn state command.Id) then
        failwith "Invalid column"
    [ ColumnMoved { Id = command.Id; Position = command.TargetPosition } ]

let handle command state  =
    match command with
    | CreateBoardCommand createBoardCommand -> createBoard createBoardCommand state
    | CreateColumnCommand createColumnCommand -> createColumn createColumnCommand state
    | MoveColumnCommand moveColumnCommand -> moveColumn moveColumnCommand state

let evolve event state =
    match event with
    | BoardCreated bc ->
        { Id = bc.Id; Name = bc.Name; Columns = []; Created = true }
    | ColumnCreated cc ->
        let numberOfColumns = state.Columns.Length
        let column = { Id = ColumnId numberOfColumns; Name = cc.Name; Cards = [] }
        { state with Columns = List.append state.Columns [ column ]  }

        
