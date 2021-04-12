import * as express from "express";
const bodyParser = require('body-parser');

const app = express();
app.use(bodyParser.urlencoded({ extended: true }));
const port = process.env.PORT || 3000;

const router = express.Router();

router.get('/available',
    async (req, res) => {
    res.send("available")
});

router.get('/notavailable', async (req, res) => {
    res.send("not available")
});

router.get('/exampleJSON', async (req, res) => {
    res.send({
        "highScores": [
            {"player": "JER", "score": 6832},
            {"player": "OAK", "score": 14},
            {"player": "WIN", "score": 5828},
            {"player": "GOW", "score": 473},
            {"player": "OIC", "score": 105},
            {"player": "GUW", "score": 12931}
        ]
    });
})

app.use("/", router);

app.listen(port, () => {
    console.log(`Listening at http://localhost:${port}`);
});
