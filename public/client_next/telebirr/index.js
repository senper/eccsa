import axios from "axios";
import { jsonSort, sha256, rsa_encrypt, rsa_decrypt } from "./commonfns.js";
import { TeleBirrConfig, TeleBirrConfigProd } from "./payment-config.js";
import { v4 as uuidv4 } from "uuid";
const TeleBirrConfigCurr = TeleBirrConfigProd; //TeleBirrConfigProd
//

let timeStamp = null;
let nonce = null;
export async function payByTelebirr(req) {
  timeStamp = Date.now();
  nonce = uuidv4().replaceAll("-", "");

  const outTradeNo = req.invoiceNo;
  const subject = req.paymentReason;
  const totalAmount = req.amount;
  const returnUrl = req.returnUrl;
  const stringA = step1(outTradeNo, subject, totalAmount, returnUrl);
  //console.log("stringA", stringA);
  const sign = step23(stringA);
  //console.log("sign", sign);
  const ussdjson = step4(outTradeNo, subject, totalAmount, returnUrl);
  //console.log("ussdjson", ussdjson);
  const ussd = step5(ussdjson);
  //console.log("ussd", ussd);

  return await step67(sign, ussd);
  step67(sign, ussd)
    .then((res) => {
      //console.log("res",res);
      //console.log(res.data);
      return res.data;
    })
    .catch((error) => {
      //console.error(error);
      return error;
    });
}

export async function notifyTelebirr(req) {
  var api = TeleBirrConfigCurr.PostApi;

  //console.log("Param ", req.body);
  const jsonRes = rsa_decrypt(req.body);
  //console.log("Param de", jsonRes);
  let requestMessage = {
    code: req.body.code,
    msg: req.body.msg,
    data: { outTradeNo: jsonRes.outTradeNo, tradeNo: jsonRes.tradeNo },
  };
  //console.log(api, requestMessage);
  try {
    const res = await axios
      .post(api, requestMessage);
    return res.data;
  } catch (error) {
    return error;
  }
}

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
  //console.log(signObj);
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
  //console.log(requestMessage);
  const api = TeleBirrConfigCurr.H5;
  return axios.post(api, requestMessage);
}
