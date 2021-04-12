"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const express = require("express");
const bodyParser = require('body-parser');
const app = express();
app.use(bodyParser.urlencoded({ extended: true }));
const port = process.env.PORT || 3000;
const router = express.Router();
router.get('/available', (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    res.send("available");
}));
router.get('/notavailable', (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    res.send("not available");
}));
router.get('/exampleJSON', (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    res.send({
        "highScores": [
            { "player": "JER", "score": 6832 },
            { "player": "OAK", "score": 14 },
            { "player": "WIN", "score": 5828 },
            { "player": "GOW", "score": 473 },
            { "player": "OIC", "score": 105 },
            { "player": "GUW", "score": 12931 }
        ]
    });
}));
app.use("/", router);
app.listen(port, () => {
    console.log(`Listening at http://localhost:${port}`);
});
