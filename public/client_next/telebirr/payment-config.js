export class TeleBirrConfig {
   static Name = "Ethiopian Chamber of commerce and sectoral associations";
   static AppId ="677194033a2342658ceb1db29c7caa6f";
   static AppKey = "e3b25848db634596ab421e3d5515f6a4";
   static ShortCode = 220185;
   static PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhxuHPN7/a+gskJEWBDBHq9Z5jMMTadiJm7uNBFYNIBk2fMFgxbjf/oqUit7OmpL3uihj0cMrKhfTmTQgHqvq5ZOGdYEPiA/nsGB2CrRQSRDNBrpbUZhLWwAeSHlbExSewPYgRGRSZv/wlyXbq16GmlhUYaiXNPJtcH954sB8GwEJmPLu2pvxaT03s1ScxerN041vuHHjrMSg7xGXnaviP90WLP/k/QHfkx1t8sFISTVGIW0F6QRFoOw5/1y/C7IdzGHZAyuHKQsKEMOk/YG6mgdjHYMDfwMh4MuTrfsVM76tDZYBT+aN8hxj+l2qvmLLz8dHKDW6ux0yBPqVx5ilQwIDAQAB";
   static H5 = "http://196.188.120.3:11443/service-openup/toTradeWebPay";//`http://192.168.80.10/api/ApiTeleMoke/GetPayUrl`;// 
   static InAppPayment = "http://196.188.120.3:11443/ammapi/service-openup/toTradeMobilePaywith";
   static TimeoutExpress = "30";
   static PostApi = `http://localhost:5000/api/ApiTeleMoke/PayNotify`; // `http://192.168.80.10/api/ApiTeleMoke/PayNotify`;
}
export class TeleBirrConfigProd {
   static Name = "Ethiopian Chamber of commerce and sectoral associations";
   static AppId ="cc0e9808b5184be7aa64a4bc34915b30";
   static AppKey = "85e0ed95a93c4bdd9d34e24359f6f24a";
   static ShortCode = 500353;
   static PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlsB+H7L4MQU1sw1YV5AVSbWJYEPMrAUU7JzPHa1cR6BJbeniHBbc47uJMLM7qgMm4ewhaScj1DGwHneE6rD1w4ZeZdBH1qV0x6G7MVLe7//6Oi4FlNPHqeSAdh4RPTdoI3EaJJed7jclCbhVDR3lFvfP3DoRTPMY7R1ivegFyQWJMDaI9/9kzTj3nhKFptfrmxh5XJYZiFVY+TfdRNn9SxyVzes6dfHW9tXuLeoVuEkL1je0t3soFX46JdCL2f9EudLJWWrHKWCD/IOzOH68ZRAwjRLBT3H3SP13gW9I/X5PTT66TGf1ybIHFHMvM+vT+oZbgjNf/+q7YNXEnYmJ+wIDAQAB";
   static H5 = "https://app.ethiomobilemoney.et:2121/ammapi/payment/service-openup/toTradeWebPay";//`http://192.168.80.10/api/ApiTeleMoke/GetPayUrl`;// 
   static InAppPayment = "https://app.ethiomobilemoney.et:2121/ammapi/payment/service-openup/toTradeMobilePay";
   static SDK = "https://app.ethiomobilemoney.et:2121/ammapi/payment/service-openup/toTradeSDKPay";
   static TimeoutExpress = "30";
   static PostApi = `http://localhost:5000/api/ApiTeleMoke/PayNotify`; // `http://192.168.80.10/api/ApiTeleMoke/PayNotify`;
}