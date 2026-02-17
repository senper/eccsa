import crypto from 'crypto';
import NodeRSA from 'node-rsa';
import { TeleBirrConfig, TeleBirrConfigProd } from './payment-config.js';

const TeleBirrConfigCurr = TeleBirrConfig; //TeleBirrConfigProd

export function jsonSort(jsonObj) {
  let arr = [];
  for (var key in jsonObj) {
    arr.push(key);
  }
  arr.sort();
  let str = "";
  for (var i in arr) {
    str += arr[i] + "=" + jsonObj[arr[i]] + "&";
  }
  return str.substring(0, str.length - 1); 
}

export function sha256(data) {
  var hash = crypto.createHash("sha256");
  hash.update(data); 
  return hash.digest("hex");
}

export const rsa_encrypt = (data) => {
  let key = new NodeRSA(getPublicKey(TeleBirrConfigCurr.PublicKey));
  key.setOptions({ encryptionScheme: "pkcs1" });
  let encryptKey = key.encrypt(data, "base64");
  return encryptKey;
};

export const rsa_decrypt = (data) => {
  let key = new NodeRSA(getPublicKey(TeleBirrConfigCurr.PublicKey));
  key.setOptions({ encryptionScheme: "pkcs1" });
  let decrypted = key.decryptPublic(data, "json");
  return decrypted;
};

export function insertStr(str, insertStr, sn) {
  var newstr = "";
  for (var i = 0; i < str.length; i += sn) {
    var tmp = str.substring(i, i + sn);
    newstr += tmp + insertStr;
  }
  return newstr;
}

export const getPublicKey = function (key) {
  const result = insertStr(key, "\n", 64);
  return "-----BEGIN PUBLIC KEY-----\n" + result + "-----END PUBLIC KEY-----";
};


export const getPrivateKey = function (key) {
  const result = insertStr(key, "\n", 64);
  return "-----BEGIN RSA PRIVATE KEY-----\n" + result + "-----END RSA PRIVATE KEY-----";
};
