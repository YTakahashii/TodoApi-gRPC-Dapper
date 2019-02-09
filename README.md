# TodoApi-gRPC-Dapper
ASP.NET CoreとgRPCとDapperを使って実装したTodoAPIです。

## DBの準備（初回）
### DB imageを起動
`$ docker-compose up -d db`

### DBとTableの作成
- `CreateDatabase.sql` を実行
- `CreateTable.sql` を実行

### Feature
この部分も自動化したい

## 起動
`$ docker-compose build; docker-compose up -d db; docker-compose up -d grpc proxy;`

## 簡易接続確認方法
### grpccをインストール（要npm）
`$ npm install -g grpcc`

### grpccを起動
`$ grpcc --proto ./Proto/todo.proto --address 127.0.0.1:5000 -i`

### Examples
### getTodoItems
#### Run
`TodoService@127.0.0.1:5000> client.getTodoItems({},pr)`

#### Result
```
TodoService@127.0.0.1:5000> 
{
  "todos": [
    {
      "Id": "983292d1bc1745a9b6b636f239e5596a",
      "Name": "Item1",
      "IsComplete": false
    }
  ]
}
```

### getTodoItem
#### Run
`TodoService@127.0.0.1:5000> client.getTodoItem({Id:"983292d1bc1745a9b6b636f239e5596a"},pr)`

#### Result
```
TodoService@127.0.0.1:5000>
{
  "todo": {
    "Id": "983292d1bc1745a9b6b636f239e5596a",
    "Name": "Item1",
    "IsComplete": false
  }
}
```

### postTodoItem
#### Run
`TodoService@127.0.0.1:5000> client.postTodoItem({Name: "TodoItem2"},pr)`

#### Result
```
TodoService@127.0.0.1:5000>
{
  "todo": {
    "Id": "17b4f25ab625454585867e294f01f601",
    "Name": "TodoItem2",
    "IsComplete": false
  }
}
```

### putTodoItem
#### Run
`TodoService@127.0.0.1:5000> client.putTodoItem({todo:{Id:"17b4f25ab625454585867e294f01f601",Name:"EditedTodoItem2", IsComplete:true}}, pr)`

#### Result
```
TodoService@127.0.0.1:5000>
{
  "todo": {
    "Id": "17b4f25ab625454585867e294f01f601",
    "Name": "EditedTodoItem2",
    "IsComplete": true
  }
}
```

### deleteTodoItem
#### Run
`TodoService@127.0.0.1:5000> client.deleteTodoItem({Id:"17b4f25ab625454585867e294f01f601"},pr)`

#### Result
```
TodoService@127.0.0.1:5000>
{
  "todo": {
    "Id": "17b4f25ab625454585867e294f01f601",
    "Name": "EditedTodoItem2",
    "IsComplete": true
  }
}
```