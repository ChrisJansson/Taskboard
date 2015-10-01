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

type StateId = StateId of int

type State =
    {
        Id : StateId
        Name : string
    }
    
type BoardId = BoardId of int

type Board =
    {
        Id : BoardId
        Name : string
        Columns : Column list
        States : State list
        Created : bool
    }
    with 
    static member initial = { Id = BoardId 0; Name = "None"; Columns = []; States = []; Created = false }
    
type Command =
    | CreateBoardCommand of CreateBoardCommand
    | CreateColumnCommand of CreateColumnCommand
    | MoveColumnCommand of MoveColumnCommand
    | CreateStateCommand of CreateStateCommand

and CreateBoardCommand = 
    {
        Id : BoardId
        Name : string
    }

and CreateStateCommand =
    {
        BoardId : BoardId
        Name : string
    }

and CreateColumnCommand =
    {
        Board : BoardId
        State : StateId
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
    | StateCreated of StateCreated

and BoardCreated =
    {
        Id : BoardId
        Name : string
    }

and StateCreated =
    {
        Id : StateId
        Name : string
    }

and ColumnCreated =
    {
        Id : ColumnId
        Name : string
        State : StateId
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
    if not (state.States |> List.map (fun s -> s.Id) |> List.contains command.State) then
        failwith "State does not exist"
    [ ColumnCreated { Name = command.Name; Id = ColumnId state.Columns.Length; State = command.State } ]

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

let createState command state =
    [ StateCreated { Id = StateId state.States.Length; Name = "New state" } ]

let handle command state  =
    match command with
    | CreateBoardCommand createBoardCommand -> createBoard createBoardCommand state
    | CreateColumnCommand createColumnCommand -> createColumn createColumnCommand state
    | MoveColumnCommand moveColumnCommand -> moveColumn moveColumnCommand state
    | CreateStateCommand csc -> createState csc state

let evolve event state =
    match event with
    | BoardCreated bc ->
        { Id = bc.Id; Name = bc.Name; Columns = []; States = []; Created = true }
    | ColumnCreated cc ->
        let numberOfColumns = state.Columns.Length
        let column = { Id = ColumnId numberOfColumns; Name = cc.Name; Cards = [] }
        { state with Columns = List.append state.Columns [ column ]  }
    | StateCreated sc ->
        let createdState = { State.Id = sc.Id; Name = sc.Name } 
        { state with States = List.append state.States [ createdState ] }
