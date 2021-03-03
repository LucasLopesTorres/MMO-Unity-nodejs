var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
const { json } = require('body-parser');
const { Console } = require('console');
var shortId = require('shortid'); //Gera ids aleatorios

var clientsInServer = []; //Lista com os jogadores no servidor

//Criando conexão
io.on('connection', function (socket) {

    var current_player;
    var login = false;

    socket.on("Join", function (pack) {

        console.log('Jogador entrou na sala' + pack.name);

        login = true;

        current_player = {
            name: pack.name,
            char: pack.char, //Código do personagem escolhido pelo jogador
            id: socket.id
        }

        clientsInServer[current_player.id] = current_player;
        socket.emit("Join_Success", current_player); //Avisa para o jogador que a conexão foi bem sucedida

        socket.broadcast.emit("Spawn_Player", current_player);//Avisa aos jogadores que um novo jogador entrou

        //Instancia os jogadores já presentes para o novo jogador
        for (client in clientsInServer) {
            if (clientsInServer[client] != current_player) {
                socket.emit("Spawn_Player", clientsInServer[client]);
            }
        }

    });

    socket.on("disconnect", function () {
        console.log("Jogador desconectado");

        if (login) {
            var data = {
                id: current_player.id
            }

            //Notifica todos os jogadores sobre a saida do jogador desconectado
            socket.broadcast.emit("User_Disconnected", data);
            //Retira o jogador da lista de jogadores do servidor
            delete clientsInServer[current_player.id];
        }
    });

    socket.on("Move", function (pack) {

        console.log("Algum jogador se movimentou");

        //Avisa aos demais jogadores sobre o movimento do jogador atual
        var data = {
            id: current_player.id,
            posic: pack["position"],
            rot: pack["rotation"]
        }

        socket.broadcast.emit("PlayerMove", data);

    });

    //Sistema de chat
    socket.on("Chat_Message", function (pack) {

        console.log("Mensagem de chat recebida");

        var data = {
            id: current_player.id,
            message: pack["message"]
        }

        //Avisa ao jogador que sua mensagem chegou ao servidor
        socket.emit("MessageSended", data);
        //avisa aos demais jogadores a mensagem recebida
        socket.broadcast.emit("MessageReceived", data);
    })

})

http.listen(3000, function () {

    console.log('server listen on 3000!');

})