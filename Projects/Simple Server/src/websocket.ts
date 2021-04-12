import Express from "express";
import path from "path";
import http from "http";
import {log} from "util";

// @ts-ignore
const allClients: SocketIOClient.Socket[] = [];

const port = process.env.PORT || 5000;
const expApp: Express.Application = Express();
const httpServer = new http.Server(expApp);
const server = httpServer.listen(port);

process.title = "Websocket Server";

secureApp(expApp);
initStaticPaths(expApp);
initRoutes(expApp);
prepareForShutdown();

const io = require("socket.io")(server);

io.sockets.on("connection", (socket: SocketIOClient.Socket) => {
    allClients.push(socket);
    console.log(
        "Got connection, server now has " + allClients.length + " players."
    );

    // DO something with the socket.id
    addToWaitingRoom(socket.id);

    socket.on("leaveRoom", function() {
        const roomWithPlayer = roomContainingPlayer(socket.id);
        if (roomWithPlayer !== undefined)
            roomWithPlayer.removePlayer(socket.id);
        addToWaitingRoom(socket.id);
    });

    socket.on("disconnect", function() {
        const i = allClients.indexOf(socket);

        if (i !== -1) {
            removePlayer(socket.id);

            allClients.splice(i, 1);

            console.log(
                "Got disconnect, server now has " + allClients.length + " players."
            );
        }
    });
});

function secureApp(app: Express.Application) {
    app.use(helmet());
}

function initStaticPaths(app: Express.Application) {
    const jsDir = path.resolve(__dirname, "static/js/");
    const cssDir = path.resolve(__dirname, "static/css/");
    const mediaDir = path.resolve(__dirname, "static/media/");

    app.use("/static/js/", Express.static(jsDir));
    app.use("/static/css/", Express.static(cssDir));
    app.use("/static/media/", Express.static(mediaDir));
}

function prepareForShutdown() {
    const signals: Array<NodeJS.Signals> = ["SIGTERM", "SIGINT"];

    const shutdownServer = () => {
        log("Shutting down server.");
        server.close();
        signals.forEach(sig => process.off(sig, shutdownServer));
        process.exit(0);
    };

    signals.forEach(sig => process.once(sig, shutdownServer));
}

function initRoutes(app: Express.Application) {
    app.get("/ios-app", function(req, res) {
        res.send("OK.");
    });
}
