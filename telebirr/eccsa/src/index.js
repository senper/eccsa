import express from "express";
import bodyParser from "body-parser";
import axios from "axios";
import { jsonSort, sha256, rsa_encrypt, rsa_decrypt } from "./commonfns.js";
import { TeleBirrConfig, TeleBirrConfigProd } from "./payment-config.js";
import { v4 as uuidv4 } from "uuid";
import cors from "cors";
import dotenv from "dotenv";
dotenv.config(); //{ path: `.env.${process.env.NODE_ENV}` });

const TeleBirrConfigCurr = TeleBirrConfigProd; //TeleBirrConfigProd
//
const app = express();
const port = 4000; //process.env.PORT;
const whitelist = [
  "http://ethiopianchamber.et",
  "http://196.188.123.107",
  "http://192.168.80.11",
  "http://localhost:3000",
];
// app.use(
//   cors({
//     origin: function (origin, callback) {
//       console.log("origin", origin);
//       if (whitelist.indexOf(origin) !== -1) {
//         callback(null, true);
//       } else {
//         callback(new Error("Not ccc allowed by CORS"));
//       }
//     },
//   })
// );

app.use(
  cors({
    origin: "*",
  })
);
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());
app.use(bodyParser.text());

let timeStamp = null;
let nonce = null;
app.post("/payByTelebirr", (req, pres) => {
  timeStamp = Date.now();
  nonce = uuidv4().replaceAll("-", "");
  // callerId: ServiceType, Membership, Resource => ReturnUrl
  const origin = req.get("origin");
  if (whitelist.indexOf(origin) === -1) {
    console.log("origin", origin, " not in white list");
    pres.send("Bad Request");
    return;
  }
  console.log("Got body:", req.body, timeStamp, "nnn", nonce);
  const outTradeNo = req.body.invoiceNo;
  const subject = req.body.paymentReason;
  const totalAmount = req.body.amount;
  const returnUrl = req.body.returnUrl;
  const stringA = step1(outTradeNo, subject, totalAmount, returnUrl);
  console.log("stringA", stringA);
  const sign = step23(stringA);
  console.log("sign", sign);
  const ussdjson = step4(outTradeNo, subject, totalAmount, returnUrl);
  console.log("ussdjson", ussdjson);
  const ussd = step5(ussdjson);
  console.log("ussd", ussd);
  //pres.send(step67(sign, ussd));
  step67(sign, ussd)
    .then((res) => {
      //console.log(res);
      console.log(res.data);
      pres.send(res.data);
    })
    .catch((error) => {
      //console.error(error);
      pres.send(error);
    });
});

app.post("/notifyTelebirr", (req, pres) => {
  var api = TeleBirrConfigCurr.PostApi;
  /* let requestMessage = {
    code: 0,
    msg: "success",
    data: { outTradeNo: "xxx", tradeNo: "WEER333350" },
  }; 
  {"msisdn":"251900000032",
  "outTradeNo":"29V85W4ASCS4_08134057",
  "totalAmount":"1",
  "tradeDate":1646735408086,
  "tradeNo":"202203081329071501142963709214721",
  "tradeStatus":2,"transactionNo":"9C810NCIEJ"}
  */
  console.log("Param ", req.body);
  const jsonRes = rsa_decrypt(req.body);
  console.log("Param de", jsonRes);
  let requestMessage = {
    code: req.body.code,
    msg: req.body.msg,
    data: { outTradeNo: jsonRes.outTradeNo, tradeNo: jsonRes.tradeNo },
  };
  console.log(api, requestMessage);
  return axios
    .post(api, requestMessage)
    .then((res) => {
      //console.log(res);
      pres.send(res.data);
    })
    .catch((error) => {
      // console.error(error);
      pres.send(error);
    });
});

app.listen(port, () => {
  console.log(`app listening at http://localhost:${port}`);
});
function step1(outTradeNo, subject, totalAmount, returnUrl) {
  //Step 1:
  const appKey = TeleBirrConfigCurr.AppKey;
  let signObj = {
    appId: TeleBirrConfigCurr.AppId,
    nonce: nonce,
    notifyUrl: `http://196.188.123.108:7531/notifyTelebirr`,
    outTradeNo: outTradeNo,
    receiveName: TeleBirrConfigCurr.Name,
    returnUrl: returnUrl,
    shortCode: TeleBirrConfigCurr.ShortCode,
    subject: subject,
    timeoutExpress: TeleBirrConfigCurr.TimeoutExpress,
    timestamp: timeStamp,
    totalAmount: totalAmount,
  };
  signObj.appKey = appKey;
  console.log(signObj);
  let StringA = jsonSort(signObj);
  return StringA;
}

function step23(StringA) {
  // Step 2:
  let StringB = sha256(StringA);

  // Step 3:
  let sign = StringB.toUpperCase();
  return sign;
}

function step4(outTradeNo, subject, totalAmount, returnUrl) {
  // Step 4:

  let jsonObj = {
    appId: TeleBirrConfigCurr.AppId,
    nonce: nonce,
    notifyUrl: `http://196.188.123.108:7531/notifyTelebirr`,
    outTradeNo: outTradeNo,
    receiveName: TeleBirrConfigCurr.Name,
    returnUrl: returnUrl,
    shortCode: TeleBirrConfigCurr.ShortCode,
    subject: subject,
    timeoutExpress: TeleBirrConfigCurr.TimeoutExpress,
    timestamp: timeStamp,
    totalAmount: totalAmount,
  };
  //jsonObj.appKey= TeleBirrConfigCurr.AppKey;
  //let StringA = jsonSort(jsonObj);
  //console.log("StringA2", StringA);
  //let StringB = sha256(StringA);
  // let sign = StringB.toUpperCase();
  //console.log("sign2", sign);
  let ussdjson = JSON.stringify(jsonObj);
  return ussdjson;
}

function step5(ussdjson) {
  // Step 5
  let ussd = rsa_encrypt(ussdjson);
  return ussd;
}

function step67(sign, ussd) {
  // Step 6
  let requestMessage = {
    appid: TeleBirrConfigCurr.AppId,
    sign: sign,
    ussd: ussd,
  };

  // Step 7:
  //return "xxx";
  console.log(requestMessage);
  const api = TeleBirrConfigCurr.H5;
  return axios.post(api, requestMessage);
}
