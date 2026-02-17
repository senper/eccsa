import axios from "axios";
import { notifyTelebirr, payByTelebirr } from "../telebirr";

const config = {
  clientId: "react.client",
  initiatePay: `${process.env.NEXT_PUBLIC_TELEAPI_URL}/payByTelebirr`,
  sendNotifyPay: `${process.env.NEXT_PUBLIC_TELEAPI_URL}/notifyTelebirr`,
  scopes: [],
};

export const initiatePay =  (invoiceNo, paymentReason, amount, returnUrl) => {
  return axios.post(config.initiatePay, { invoiceNo, paymentReason, amount, returnUrl });
  //return await payByTelebirr({ invoiceNo, paymentReason, amount, returnUrl });
};

export const sendNotify = (invoiceNo, tradeNo) => {
  return axios.post(config.sendNotifyPay, { invoiceNo, tradeNo });
  //return notifyTelebirr({ invoiceNo, tradeNo });
};
