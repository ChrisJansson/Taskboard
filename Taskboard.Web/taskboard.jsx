var Taskboard = React.createClass({
	getInitialState: function() {
		return {data: this.props.taskboard};
	},
	handleColumnCreateSubmit: function(column) {
		var newColumn = {
			name: column.name,
			cards: []
		};
		
		var board = this.state.data;
		var newBoard = React.addons.update(board, {
			columns: { $push: [newColumn] }
		});
		
		this.setState({data: newBoard});
		return;
	},
	handleMoveCard: function(sourceColumnId, targetColumnId, targetCardId) {
		if(sourceColumnId === targetColumnId)
			return;
	
		var board = this.state.data;
		var findColumn = function (columnId) {
			return function (column) {
				return column.id === columnId;
			};
		};
	
		var sourceColumnIndex = board.columns.findIndex(findColumn(sourceColumnId));
		var targetColumnIndex = board.columns.findIndex(findColumn(targetColumnId));
		var sourceColumn = board.columns[sourceColumnIndex];
		var targetColumn = board.columns[targetColumnIndex];
			
		var newSourceColumnCards = sourceColumn.cards.filter(function (card) {
			return card.id !== targetCardId
		});
		
		var newSourceColumn = React.addons.update(sourceColumn, {
			cards: { $set : newSourceColumnCards }
		});
		
		var card = sourceColumn.cards.find(function (card) {
			return card.id === targetCardId;
		});
		
		newTargetColumn = React.addons.update(targetColumn, {
			cards: { $push: [ card ] }
		});
				
		var newBoard = React.addons.update(board, {
			columns: { 
				$splice: [[ sourceColumnIndex, 1, newSourceColumn ], [ targetColumnIndex, 1, newTargetColumn ]]
			 }
		});
		
		this.setState({data: newBoard});
	},
	render: function() {
		var columnNodes = this.state.data.columns.map(function (column) {
			return (
				<TaskboardColumn data={column} onMoveCard={this.handleMoveCard} />
			);
		}, this);
		return (
			<div className="taskboard">
				<div className="taskboardColumns">
					{columnNodes}
				</div>
				<TaskboardAddColumnForm onColumnCreateSubmit={this.handleColumnCreateSubmit} />
			</div>
		);
	}
});

var TaskboardColumn = React.createClass({
	handleOnDragStart: function(e, card) {
		e.dataTransfer.effectAllowed = "move";
		var moveCard = {
			columnId: this.props.data.id,
			card: card
		};
		e.dataTransfer.setData("text", JSON.stringify(moveCard));
	},
	handleOnDragOver: function(e) {
		e.dataTransfer.dropEffect = "move";
		e.preventDefault();
	},
	handleOnDrop: function(e) {
		var moveCard = JSON.parse(e.dataTransfer.getData("text"));	
		this.props.onMoveCard(moveCard.columnId, this.props.data.id, moveCard.card.id);
	},
	render: function() {
		var cardNodes = this.props.data.cards.map(function (card) {
			return (
				<TaskboardCard data={card} onCardDragStart={this.handleOnDragStart} onDragStart={this.handleOnDragStart} />
			);
		}, this);
		return (
			<div className="taskboardColumn panel panel-default" onDragOver={this.handleOnDragOver} onDrop={this.handleOnDrop}>
				<div className="panel-heading taskboardColumnHeader">
					{this.props.data.name}
				</div>
				<div>
					{cardNodes}
				</div>
			</div>
		);
	}
});

var TaskboardCard = React.createClass({
	handleOnDragStart: function(e) {
		this.props.onDragStart(e, this.props.data);
	},
	render: function() {
		return (
			<div className="taskboardCard panel panel-primary" draggable="true" onDragStart={this.handleOnDragStart}>
				<div className="taskboardColumnCardHeader panel-heading">
					{this.props.data.name}
				</div>
				{this.props.data.text}
			</div>
		);
	}
});

var TaskboardAddColumnForm = React.createClass({
	handleSubmit: function(e) {
		e.preventDefault();
		var name = React.findDOMNode(this.refs.name).value.trim();
		this.props.onColumnCreateSubmit({name: name});
		return;
	},
	render: function() {
		return (
			<div>
				<form onSubmit={this.handleSubmit}>
					<div>
						<label for="name">Name</label>
						<input id="name" ref="name" type="text" />
					</div>
					<div>
						<input type="submit" value="Add column" />
					</div>
				</form>
			</div>
		);
	}
});