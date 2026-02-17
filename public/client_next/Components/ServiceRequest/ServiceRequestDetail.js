import React from "react";
import {
  getServiceRequest,
  submitRequest,
  submitDocument,
  closeDocument,
  savePayment,
  printInvoice,
  printReceipt,
  saveRequirementPayment,
  printCertificate,
  saveTeleOutTradeNo,
} from "../../api/serviceRequest";
import Link from "next/link";
import { initiatePay } from "../../api/telepay";
import { useForm, Controller } from "react-hook-form";
import { useRouter } from "next/router";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Grid,
  Snackbar,
} from "@mui/material";
import { AddAlert } from "@mui/icons-material";
import ConfirmDialog from "../Common/ConfirmDialog";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { getServiceType } from "../../pages/api/servicetype";
import ServiceRequestDetailTbl from "./ServiceRequestDetailTbl";
import ServiceRequestAttachments from "./ServiceRequestAttachments";
import ServiceRequestFollowupTbl from "./ServiceRequestFollowupTbl";
import ServiceRequestOptionals from "./ServiceRequestOptionals";

export default function ServiceRequestDetail({ getLocal, getLocalByVal }) {
  const location = useRouter();
  ////
  const [loading, setLoading] = React.useState(false);
  const { register, control, setValue, getValues } = useForm();
  //
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(false);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg("");
          }, 6000);
        }
        break;
      default:
        break;
    }
  };
  //
  const [downloadCertificateLabel, setDownloadCertificateLabel] =
    React.useState(false);
  const [openConfirm, setOpenConfirm] = React.useState(false);
  const [savingOutTradeNo, setSavingOutTradeNo] = React.useState(false);
  const [dialogText, setDialogText] = React.useState(<></>);
  const [resultAction, setResultAction] = React.useState(null);
  const [btnValue, setButtonValue] = React.useState(getLocal("Submit"));
  const [param, setParam] = React.useState(null);
  ///
  let serviceTypeIdValue = 0;
  let serviceRequestIdValue = null;
  if (location.query) {
    const { serviceTypeId, serviceRequestId } = location.query;
    serviceTypeIdValue = serviceTypeId;
    serviceRequestIdValue = serviceRequestId;
  }
  ///
  const [serviceType, setServiceType] = React.useState(null);
  React.useEffect(() => {
    if (serviceTypeIdValue) {
      getServiceType(serviceTypeIdValue).then((data) => {
        setServiceType(data.data);
      });
    }
  }, [serviceTypeIdValue]);
  ///
  const [serviceRequest, setServiceRequest] = React.useState(null);
  const [serviceRequestDocs, setServiceRequestDocs] = React.useState([]);
  function loadRequest() {
    setLoading(true);
    getServiceRequest(serviceRequestIdValue)
      .then((data) => {
        setServiceRequest(data.data);
        setServiceRequestDocs(
          data.data.responses && data.data.responses.$values
            ? data.data.responses.$values
                .filter(
                  (v) =>
                    v.requirement && v.requirement.requirementTypeValue != 2
                )
                .sort(compare)
            : []
        );
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }
  React.useEffect(() => {
    if (serviceRequestIdValue) {
      loadRequest();
    }
  }, [serviceRequestIdValue]);
  ///
  const [reqPaymentDialogStates, setReqPaymentDialogStates] = React.useState(
    []
  );
  const [reqPaymentDialogPayMethod, setReqPaymentDialogPayMethod] =
    React.useState([]);
  function OpenPaymentDialog(id) {
    const newFeatures = [...reqPaymentDialogStates];
    newFeatures[id] = true;
    setReqPaymentDialogStates(newFeatures);
  }
  function ClosePaymentDialog(id) {
    const newFeatures = [...reqPaymentDialogStates];
    newFeatures[id] = false;
    setReqPaymentDialogStates(newFeatures);
  }
  function RequirementReceptForm(row) {
    if (
      !row.invoiceNo ||
      (row.receiptNo && row.paymentStatusInt == 0) ||
      row.paymentStatusInt == 1
    ) {
      return null;
    }
    return (
      <>
        <button
          type="button"
          className="btn-sm btn btn-warning"
          style={{ float: "none", margin: "5px 0px 0 10px" }}
          onClick={() => OpenPaymentDialog(row.requirementId)}
        >
          {getLocal("Payment")}
        </button>
        {reqPaymentDialogStates[row.requirementId] ? (
          <Dialog
            open={reqPaymentDialogStates[row.requirementId]}
            //onClose={() => ClosePaymentDialog(row.requirementId)}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            tabIndex={0}
          >
            <DialogTitle id="alert-dialog-title" className="alert-info">
              {`${getLocal("Payment for")} ${getLocalByVal(
                row.requirement.label,
                row.requirement.labelLocal
              )} `}
            </DialogTitle>
            <DialogContent>
              <DialogContentText id="alert-dialog-description">
                {/* <form onSubmit={handleSubmit(onRequirementReceptSubmit)}> */}
                {getLocal("Invoice No")}:<strong>{" " + row.invoiceNo}</strong>
                <br />
                <br />
                {getLocal("Receipt No")}:{" "}
                <input
                  type="text"
                  className="form-control"
                  //disabled={reqPaymentDialogPayMethod[row.requirementId] != 1}
                  {...register(row.requirementId + "receiptNo", {
                    required: true,
                    maxLength: 50,
                  })}
                />
                <br />
                {getLocal("Receipt")}:{" "}
                <input
                  type="file"
                  className="form-control"
                  //disabled={reqPaymentDialogPayMethod[row.requirementId] != 1}
                  required
                  {...register(row.requirementId + "receiptAtt", {
                    required: true,
                  })}
                />
                <br />
                {getLocal("Amount")}:{" "}
                <input
                  type="number"
                  disabled
                  min="0"
                  step="any"
                  className="form-control"
                  value={row.remainingAmount}
                />
                <br />
                {getLocal("Payment Options")}: <br />
                <Controller
                  control={control}
                  name={row.requirementId + "paymentMethod"}
                  render={({ field: { onChange, onBlur, value, ref } }) => (
                    <input
                      type="radio"
                      checked="checked"
                      value="1"
                      disabled={
                        row.invoice &&
                        row.invoice.telePayUrl &&
                        row.invoice.telePayUrl.length > 0
                      }
                      name={row.requirementId + "paymentMethod"}
                      onChange={(e) => {
                        onChange(e);
                        setValue(row.requirementId + "paymentMethod", 1);
                        const newFeatures = [...reqPaymentDialogPayMethod];
                        newFeatures[row.requirementId] = 1;
                        setReqPaymentDialogPayMethod(newFeatures);
                      }}
                    />
                  )}
                />{" "}
                <span
                  style={{ fontSize: "16px", color: "inherit" }}
                  title={getLocal(
                    "Includes Internet Banking, Bank Transfer, Cash Payment"
                  )}
                >
                  1, {getLocal("Other")}
                </span>{" "}
                {"   "}
                <Controller
                  control={control}
                  name={row.requirementId + "paymentMethod"}
                  render={({ field: { onChange, onBlur, value, ref } }) => (
                    <input
                      type="radio"
                      disabled="disabled"
                      name={row.requirementId + "paymentMethod"}
                      value="2"
                      onChange={(e) => {
                        onChange(e);
                        setValue(row.requirementId + "paymentMethod", 2);
                        setValue(row.requirementId + "receiptNo", "");
                        setValue(row.requirementId + "receiptAtt", "");
                        const newFeatures = [...reqPaymentDialogPayMethod];
                        newFeatures[row.requirementId] = 2;
                        setReqPaymentDialogPayMethod(newFeatures);
                      }}
                    />
                  )}
                />{" "}
                2,{" "}
                <img
                  src="/images/telebirr2.jpg"
                  width={"75px"}
                  alt="telebirr"
                />
                {"  "}
                {/* </form> */}
              </DialogContentText>
            </DialogContent>
            <DialogActions>
              <button
                type="button"
                className="btn btn-sm btn-outline-info"
                onClick={() => ClosePaymentDialog(row.requirementId)}
                style={{ float: "none", margin: "0 10px 0 0" }}
              >
                {getLocal("Cancel")}
              </button>
              <button
                type="button"
                className="btn-sm btn btn-warning"
                style={{ float: "none", margin: "5px 0px 0 10px" }}
                disabled={savingOutTradeNo}
                onClick={() =>
                  onRequirementReceptSubmit(
                    row.requirementId,
                    row.remainingAmount,
                    row.invoiceNo,
                    getLocalByVal(
                      row.requirement.label,
                      row.requirement.labelLocal
                    ),
                    row.invoice.telePayUrl,
                    row.id
                  )
                }
              >
                {savingOutTradeNo ? getLocal("Saving") : getLocal("Save")}
              </button>
            </DialogActions>
          </Dialog>
        ) : null}
      </>
    );
  }
  function onRequirementReceptSubmit(
    id,
    amount,
    invoiceNo,
    reqLabel,
    toPayUrl,
    responseId
  ) {
    const values = getValues();
    const reqPaymentMethod = 1; //values[id + "paymentMethod"];
    if (reqPaymentMethod == 2) {
      TeleBirrPayment(invoiceNo, amount, reqLabel, responseId, toPayUrl);
      return;
    }
    if (!reqPaymentMethod) {
      setErrorMsg(getLocal("Select Payment Method"));
      showNotification("tr");
      return;
    }
    const reqReceiptNo = values[id + "receiptNo"];
    if (!values[id + "receiptAtt"]) {
      setErrorMsg(getLocal("Attach the receipt!"));
      showNotification("tr");
      return;
    }
    const reqReceiptAttachment = values[id + "receiptAtt"][0];
    if (reqPaymentMethod == 1) {
      if (!reqReceiptNo) {
        setErrorMsg(getLocal("Enter a valid Receipt No"));
        showNotification("tr");
        return;
      }
      if (!reqReceiptAttachment) {
        setErrorMsg(getLocal("Attach the receipt!"));
        showNotification("tr");
        return;
      }
    }
    setOpenConfirm(true);
    const data = {
      requestId: serviceRequest.id,
      id: id,
      receiptNo: reqReceiptNo,
      receiptAttachment: reqReceiptAttachment,
      paymentMethod: reqPaymentMethod,
      invoiceNo: invoiceNo,
      amount: amount,
      remark: "",
      reqLabel: reqLabel,
      toPayUrl: toPayUrl,
      responseId,
      responseId,
    };
    setParam(data);
    setResultAction(() => ConfirmSaveRequirementPayment);
    setDialogText(
      <>
        {getLocal("Save Payment")} {getLocal("with receipt No")}{" "}
        <strong>{reqReceiptNo}</strong> & {getLocal("payment method")}{" "}
        <strong>
          {" "}
          {reqPaymentMethod == 1 ? getLocal("Other") : getLocal("telebirr")}
        </strong>{" "}
        {getLocal("with amount")} <strong> {amount}</strong>
      </>
    );
  }
  function ConfirmSaveRequirementPayment(param) {
    if (param.paymentMethod == 2) {
      TeleBirrPayment(
        param.receiptNo,
        param.amount,
        param.reqLabel,
        param.responseId,
        param.toPayUrl
      );
      return;
    }
    setOpenConfirm(false);
    var formData = new FormData();
    formData.append("id", param.id);
    formData.append("requestId", param.requestId);
    formData.append("receiptNo", param.receiptNo);
    formData.append("receipt", param.receiptAttachment);
    formData.append("paymentMethod", param.paymentMethod);
    formData.append("invoiceNo", param.invoiceNo);
    formData.append("amount", param.amount);
    formData.append("remark", param.remark);
    saveRequirementPayment(serviceRequest.id, formData)
      .then((data) => {
        ClosePaymentDialog(param.id);
        loadRequest();
      })
      .catch((error) => {
        // console.log(error.response);
        if (error.response) {
          setErrorMsg(error.response.data);
        }
        showNotification("tr");
      });
  }
  ///
  function compare(a, b) {
    if (a.requirement.orderNo < b.requirement.orderNo) {
      return -1;
    }
    if (a.requirement.orderNo > b.requirement.orderNo) {
      return 1;
    }
    return 0;
  }
  //
  const [receiptNo, setReceiptNo] = React.useState("");
  const [receiptAttachment, setReceiptAttachment] = React.useState(null);
  const [paymentMethod, setPaymentMethod] = React.useState(1);
  const [paymentAmt, setPaymentAmt] = React.useState(0);

  const getAttachment = (set) => {
    if (
      !set.extension ||
      set.extension.trim() == "" ||
      !serviceType.serviceOwner
    )
      return set.receiptNo;
    const fileName = set.id.substr(set.id.length - 15).replace("-", "");
    const relativePath = `${serviceType.serviceOwner.toLowerCase()}/documents/receipts/${fileName}${
      set.extension
    }`;

    return (
      <a
        href={`${process.env.NEXT_PUBLIC_API_URL}/${relativePath}`}
        target="_blank"
        rel="noreferrer"
        title={`${getLocal("Receipt")} ` + set.receiptNo}
        download={"Receipt " + set.receiptNo}
      >
        {set.receiptNo}
      </a>
    );
  };

  function DisplayLastReceipt() {
    if (
      !serviceRequest.invoice ||
      !serviceRequest.invoice.settlments ||
      !serviceRequest.invoice.settlments.$values
    ) {
      return null;
    }
    var settlment = serviceRequest.invoice.settlments.$values.find(
      (s) => s.receiptNo == serviceRequest.receiptNo
    );
    return (
      <>
        {settlment ? (
          <>
            {settlment.extension
              ? getAttachment(settlment)
              : settlment.receiptNo}{" "}
          </>
        ) : null}
      </>
    );
  }
  function DisplayOtherReceipts() {
    if (
      !serviceRequest.invoice ||
      !serviceRequest.invoice.settlments ||
      !serviceRequest.invoice.settlments.$values
    ) {
      return null;
    }
    var settlments = serviceRequest.invoice.settlments.$values.filter(
      (s) => s.receiptNo != serviceRequest.receiptNo
    );
    return (
      <>
        {settlments.length > 0 ? (
          <>
            <br />
            {getLocal("Other Receipts")}:{" "}
            {settlments.map((set, index) => (
              <span key={index}>
                {set.extension ? getAttachment(set) : set.receiptNo}{" "}
              </span>
            ))}
          </>
        ) : null}
      </>
    );
  }
  function ReceptForm() {
    if (
      !serviceRequest.invoiceNo ||
      (serviceRequest.receiptNo && serviceRequest.paymentStatusInt == 0) ||
      serviceRequest.paymentStatusInt == 1
    ) {
      return null;
    }
    return (
      <>
        <br />
        {getLocal("Receipt No")}:{" "}
        <input
          type="text"
          disabled={paymentMethod != 1}
          style={{ marginBottom: "10px", marginTop: "5px" }}
          value={receiptNo}
          onChange={(event) => setReceiptNo(event.target.value)}
        />
        <br />
        {getLocal("Receipt")}:{" "}
        <input
          type="file"
          disabled={paymentMethod != 1}
          required
          value={receiptAttachment ? receiptAttachment.fileName : ""}
          style={{ marginBottom: "10px", marginLeft: "22px" }}
          onChange={(event) => setReceiptAttachment(event.target.files[0])}
        />
        <br />
        {getLocal("Amount")}:{" "}
        <input
          type="number"
          disabled
          value={serviceRequest.remainingAmount}
          min="0"
          step="any"
          style={{ marginLeft: "18px", marginBottom: "10px" }}
          onChange={(event) => setPaymentAmt(event.target.value)}
        />
        <br />
        <div>{getLocal("Payment Options")}:</div>{" "}
        <input
          type="radio"
          checked="checked"
          name="paymentMethod"
          disabled={
            serviceRequest.invoice &&
            serviceRequest.invoice.telePayUrl &&
            serviceRequest.invoice.telePayUrl.length > 0
          }
          style={{ marginBottom: "10px", marginLeft: "60px" }}
          onChange={(event) => setPaymentMethod(1)}
        />{" "}
        <span
          style={{ fontSize: "16px" }}
          title={"Includes Internet Banking, Bank Transfer, Cash Payment"}
        >
          1, {getLocal("Other")}
        </span>{" "}
        {"  "}
        <input
          type="radio"
          disabled="disabled"
          name="paymentMethod"
          style={{ marginBottom: "10px", marginLeft: "10px" }}
          onChange={(event) => {
            setPaymentMethod(2);
            setReceiptNo("");
            setReceiptAttachment(null);
          }}
        />
        {"  "}
        2, <img src="/images/telebirr2.jpg" width={"75px"} alt="telebirr" />
        {"  "}
        <button
          type="button"
          className="btn-sm btn btn-warning"
          style={{ float: "none", margin: "5px 0px 0 10px" }}
          onClick={SavePayment}
          disabled={savingOutTradeNo}
        >
          {savingOutTradeNo ? "Saving" : "Save"}
        </button>
      </>
    );
  }
  function getDDHHMMSS() {
    const now = new Date();
    let date = now.getDate();
    date = (date > 9 ? "" : "0") + date.toString();
    let hh = now.getHours();
    hh = (hh > 9 ? "" : "0") + hh.toString();
    let min = now.getMinutes();
    min = (min > 9 ? "" : "0") + min.toString();
    let sec = now.getSeconds();
    sec = (sec > 9 ? "" : "0") + sec.toString();
    return `${date}${hh}${min}${sec}`;
  }
  function TeleBirrPayment(invoiceNo, amount, reason, refId, toPayUrl) {
    {
      let returnUrl = `${window.location.href}Param/${serviceTypeIdValue}/${serviceRequestIdValue}`;

      const outTradeNo = `${invoiceNo}_${getDDHHMMSS()}`;
      setSavingOutTradeNo(true);
      initiatePay(outTradeNo, reason, amount, returnUrl)
        .then((res) => {
          //console.log("in the deital");
          //console.log(res);
          if (res.status == 200 && res.data.code == 200) {
            //return res.data.data.toPayUrl;
            var formData = new FormData();
            formData.append("refId", refId);
            formData.append("payUrl", res.data.data.toPayUrl);
            formData.append("request", refId == serviceRequest.id);
            formData.append("outTradeNo", outTradeNo);
            saveTeleOutTradeNo(refId, formData).then((saveRes) => {
              if (res.status == 200) {
                window.open(res.data.data.toPayUrl, "_self");
              } else {
                setSavingOutTradeNo(false);
              }
            });
          } else if (
            res.status == 200 &&
            res.data.code == 103011 &&
            serviceRequest.invoice &&
            toPayUrl
          ) {
            window.open(toPayUrl, "_self");
          } else {
            //console.error(res.data.message);
            setErrorMsg(getLocal("telebirr Payment Start Failed!"));
            showNotification("tr");
            setSavingOutTradeNo(false);
          }
        })
        .catch((error) => {
          //console.log("in the deital ex");
          //console.error(error);
          setErrorMsg(getLocal("telebirr Payment Start Failed!"));
          showNotification("tr");
          setSavingOutTradeNo(false);
          return error;
        });
    }
  }
  function SavePayment() {
    if (paymentMethod == 2) {
      TeleBirrPayment(
        serviceRequest.invoiceNo,
        serviceRequest.remainingAmount,
        serviceType.name,
        serviceRequest.id,
        serviceRequest.invoice.telePayUrl
      );
      return;
    }
    if (!paymentMethod) {
      setErrorMsg(getLocal("Select Payment Method"));
      showNotification("tr");
      return;
    }
    if (paymentMethod == 1) {
      if (!receiptNo) {
        setErrorMsg(getLocal("Enter a valid Receipt No!"));
        showNotification("tr");
        return;
      }
      if (!receiptAttachment) {
        setErrorMsg(getLocal("Attach the receipt!"));
        showNotification("tr");
        return;
      }
    }
    setPaymentAmt(serviceRequest.remainingAmount);
    /* if(!paymentAmt){
      setErrorMsg("Enter a valid Amount");
      showNotification("tr");
      return;
    } */
    setOpenConfirm(true);
    setResultAction(() => ConfirmSavePayment);
    setDialogText(
      <>
        {getLocal("Save Payment")} {getLocal("with receipt No")}{" "}
        <strong>{receiptNo}</strong> & {getLocal("payment method")}{" "}
        <strong> {paymentMethod == 1 ? "Other" : "telebirr"}</strong>{" "}
        {getLocal("with amount")}{" "}
        <strong> {serviceRequest.remainingAmount}</strong>
      </>
    );
  }
  function ConfirmSavePayment() {
    if (paymentMethod == 2) {
      TeleBirrPayment(
        receiptNo,
        serviceRequest.remainingAmount,
        serviceType.name,
        serviceRequest.id,
        serviceRequest.invoice.telePayUrl
      );
      return;
    }
    setOpenConfirm(false);
    var formData = new FormData();
    formData.append("requestId", serviceRequest.id);
    formData.append("receiptNo", receiptNo);
    formData.append("receipt", receiptAttachment);
    formData.append("paymentMethod", paymentMethod);
    formData.append("invoiceNo", serviceRequest.invoiceNo);
    formData.append("amount", serviceRequest.remainingAmount);
    const data = {
      requestId: serviceRequest.id,
      receiptNo: receiptNo,
      paymentMethod: paymentMethod,
      invoiceNo: serviceRequest.invoiceNo,
      amount: paymentAmt,
    };
    savePayment(serviceRequest.id, formData)
      .then((data) => {
        loadRequest();
      })
      .catch((error) => {
        // console.log(error.response);
        if (error.response) {
          setErrorMsg(error.response.data);
        }
        showNotification("tr");
      });
  }
  //
  function CloseRequest() {
    setOpenConfirm(true);
    setResultAction(() => ConfirmCloseRequest);
    setButtonValue(getLocal("Yes"));
    setDialogText(
      <>
        {getLocal("Close request")} {getLocal("with tracking No")}{" "}
        <strong>{serviceRequest.trackingNo}</strong>
      </>
    );
  }
  function ConfirmCloseRequest() {
    setOpenConfirm(false);
    closeDocument(serviceRequest.id)
      .then((data) => {
        loadRequest();
      })
      .catch((error) => {
        // console.log(error);
        showNotification("tr");
      });
  }
  function SubmitRequest() {
    setOpenConfirm(true);
    setButtonValue(getLocal("Submit"));
    setResultAction(() => ConfirmSubmitRequest);
    setDialogText(
      <>
        {getLocal("Submit request")} {getLocal("with tracking No")}{" "}
        <strong>{serviceRequest.trackingNo}</strong>
      </>
    );
  }
  function ConfirmSubmitRequest() {
    setOpenConfirm(false);
    submitRequest(serviceRequest.id)
      .then((data) => {
        loadRequest();
      })
      .catch((error) => {
        // console.log(error);
        showNotification("tr");
      });
  }
  function SubmitDoc(res) {
    setOpenConfirm(true);
    setButtonValue(getLocal("Submit"));
    setParam(res.id);
    setResultAction(() => ConfirmSubmitDoc);
    setDialogText(
      <>
        {getLocal("Submit response")} {getLocal("for requirement")}{" "}
        <strong>
          {getLocalByVal(res.requirement.label, res.requirement.labelLocal)}
        </strong>
      </>
    );
  }
  function ConfirmSubmitDoc(id) {
    setOpenConfirm(false);
    submitDocument(id)
      .then((data) => {
        loadRequest();
      })
      .catch((error) => {
        // console.log(error);
        showNotification("tr");
      });
  }
  function GetStatusChangeButton() {
    let buttonText = "";
    let buttonAction = null;
    let btnShow = false;
    if (
      serviceRequest.requestStatusValue === 0 &&
      (serviceType.rateBasis !== "Flat" ||
        (serviceRequest.invoiceNo && serviceRequest.receiptNo)) &&
      !serviceRequest.isOpponent
    ) {
      buttonText = getLocal("Submit");
      buttonAction = SubmitRequest;
      btnShow = true;
    } else if (
      (serviceRequest.requestStatusValue === 5 ||
        serviceRequest.requestStatusValue === 6) &&
      !serviceRequest.isOpponent
    ) {
      buttonText = getLocal("Close");
      buttonAction = CloseRequest;
      btnShow = true;
    } /* else if (serviceRequest.requestStatusValue === 5) {
      buttonText = "Accept";
      buttonAction = AcceptRequest;
      btnShow = true;
      btn2Show = true;
    } */
    return btnShow ? (
      <>
        <br />
        <button
          type="button"
          className="btn-sm btn btn-warning "
          style={{ float: "none", margin: "0" }}
          onClick={() => buttonAction()}
        >
          {buttonText}
        </button>
      </>
    ) : null;
  }
  function ShowResultDocument() {
    if (
      serviceRequest.extension &&
      serviceType.serviceOwnerStr && //serviceRequest.requestStatusValue === 3 ||
      (serviceRequest.requestStatusValue === 5 ||
        //serviceRequest.requestStatusValue === 6 ||
        serviceRequest.requestStatusValue === 100)
    ) {
      const fileName = serviceRequest.id
        .substr(serviceRequest.id.length - 15)
        .replace("-", "");
      const relativePath = `${serviceType.serviceOwnerStr.toLowerCase()}/documents/${fileName}${
        serviceRequest.extension
      }`;
      return (
        <>
          <br />
          <a
            href={`${process.env.NEXT_PUBLIC_API_URL}/${relativePath}`}
            target="_blank"
            rel="noreferrer"
          >
            {getLocal("Result Document")}
          </a>
        </>
      );
    }
  }
  function ShowCertificateLink() {
    if (
      serviceType.certificateTypeValue > 0 &&
      serviceType.serviceOwnerStr &&
      (serviceRequest.requestStatusValue === 5 ||
        serviceRequest.requestStatusValue === 100)
    ) {
      return downloadCertificateLabel == true ? (
        <>{getLocal("Downloading Certificate...")}</>
      ) : (
        <a
          className="text-primary"
          style={{ cursor: "pointer" }}
          onClick={() => onPrintCertificate(serviceRequest.id)}
        >
          {getLocal("Download Certificate")}
        </a>
      );
    }
  }

  function onPrintCertificate(id) {
    setDownloadCertificateLabel(true);
    printCertificate(id)
      .then((data) => {
        const url = window.URL.createObjectURL(new Blob([data.data]));
        const link = document.createElement("a");
        link.href = url;
        //if(serviceType.certificateType == "SupportLetter") {
        //  link.setAttribute("download", `${serviceType.certificateType}.docx`); //or any other extension
        //} else {
        link.setAttribute("download", `${serviceType.certificateType}.pdf`); //or any other extension
        //}
        document.body.appendChild(link);
        link.click();
        link.remove();
        setDownloadCertificateLabel(false);
      })
      .catch((error) => {
        // console.log(error.response);
        if (error.response) {
          setErrorMsg(getLocal("Failed To Download"));
          showNotification("tr");
        }
        setDownloadCertificateLabel(false);
      });
  }

  function onPrintInvoice(id, invoiceNo) {
    printInvoice(id, invoiceNo)
      .then((data) => {
        const url = window.URL.createObjectURL(new Blob([data.data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", `Invoice_${invoiceNo}.pdf`); //or any other extension
        document.body.appendChild(link);
        link.click();
        link.remove();
      })
      .catch((error) => {
        // console.log(error.response);
        if (error.response) {
          setErrorMsg(getLocal("Failed To Download"));
          showNotification("tr");
        }
      });
  }
  function onPrintReceipt(id, invoiceNo, receiptNo) {
    printReceipt(id, invoiceNo)
      .then((data) => {
        const url = window.URL.createObjectURL(new Blob([data.data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute(
          "download",
          `Receipt_For_Invoice_${invoiceNo}_Receipt_${receiptNo}.pdf`
        ); //or any other extension
        document.body.appendChild(link);
        link.click();
        link.remove();
      })
      .catch((error) => {
        // console.log(error.response);
        if (error.response) {
          setErrorMsg(getLocal("Failed To Download"));
          showNotification("tr");
        }
      });
  }
  function ShowInvoiceDownload() {
    if (!serviceRequest.invoiceNo) {
      return null;
    }
    return (
      <a
        className="text-primary"
        style={{ cursor: "pointer" }}
        onClick={() =>
          onPrintInvoice(serviceRequest.id, serviceRequest.invoiceNo)
        }
        to={{
          state: {
            serviceRequestId: serviceRequest.id,
            serviceTypeId: serviceRequest.serviceTypeId,
          },
        }}
      >
        ({getLocal("Download Invoice")})
      </a>
    );
  }
  function ShowReceiptDownload() {
    if (
      !serviceRequest.invoiceNo ||
      !serviceRequest.receiptNo ||
      !serviceRequest.printReceipt
    ) {
      return null;
    }
    return (
      <a
        className="text-primary"
        style={{ cursor: "pointer" }}
        onClick={() =>
          onPrintReceipt(
            serviceRequest.id,
            serviceRequest.invoiceNo,
            serviceRequest.receiptNo
          )
        }
      >
        ({getLocal("Download Receipt")})
      </a>
    );
  }
  return <>
    {loading ? (
      <h2 className="section-title">
        <br />
        {getLocal("Loading")} {getLocal("Request detail")}...
      </h2>
    ) : null}
    {serviceType && serviceRequest ? (
      <>
        <Grid container>
          <Grid item xs={12}>
            <Accordion defaultExpanded={true}>
              <AccordionSummary
                style={{ height: "1px" }}
                expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
              >
                <div className="text-xs font-weight-bold text-primary text-uppercase">
                  <p className="links">
                    {" "}
                    {getLocal("Request")}{" "}
                    {getLocalByVal(
                      serviceType.nameEng,
                      serviceType.nameLocal
                    )}
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails>
                <Grid container spacing={5}>
                  <Grid item xs={12} sm={4}>
                    {getLocal("Request Date")}:{" "}
                    {serviceRequest.requestDateStr}
                    <br />
                    {getLocal("Tracking Number")}: {serviceRequest.trackingNo}
                    <br />
                    Status: {serviceRequest.requestStatus}{" "}
                    {serviceRequest.requestStatusValue == 6 ? (
                      <>
                        <br />
                        {serviceRequest.remark}
                      </>
                    ) : null}
                    {GetStatusChangeButton()}
                    {ShowResultDocument()} <br />
                    {ShowCertificateLink()}
                    <br />
                  </Grid>
                  <Grid item xs={12} sm={5}>
                    {getLocal("Invoice No")}: {serviceRequest.invoiceNo}{" "}
                    {ShowInvoiceDownload()}
                    <br />
                    {getLocal("Last Receipt No")}: <>{DisplayLastReceipt()}</>{" "}
                    {serviceRequest.paymentStatusInt == 3
                      ? "(" + serviceRequest.paymentStatus + ")"
                      : null}
                    {ReceptForm()} {ShowReceiptDownload()}
                    <br />
                    {getLocal("Confirmed Amount")}:{" "}
                    {serviceRequest.invoice
                      ? serviceRequest.invoice.totalPaid
                      : 0}
                    {DisplayOtherReceipts()}
                    <br />
                  </Grid>
                  <Grid item xs={12} sm={3}>
                    <strong>{getLocal("Bank Accounts")}</strong>
                    <br />
                    CBE: 1000000950495 (Ethiopian Chamber of Commerce and
                    Sectoral Associations)
                    <br />
                    <br />
                  </Grid>
                </Grid>
              </AccordionDetails>
            </Accordion>
          </Grid>
          <Grid item xs={12}>
            <Accordion defaultExpanded={true}>
              <AccordionSummary
                style={{ height: "1px" }}
                expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
              >
                <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                  <p className="links">
                    {" "}
                    {getLocal("Request Documents")}{" "}
                    {serviceRequest.rejectedDocumentsNo > 0 ||
                    serviceRequest.requestStatusValue === 0 ||
                    (serviceRequest.isOpponent &&
                      serviceRequest.requestStatusValue === 2 &&
                      serviceRequest.acceptedDocumentsNo <
                        serviceRequest.totalDocumentsNo) ? (
                      <Link
                        href={`/myServices/serviceRequestForm?serviceTypeId=${serviceRequest.serviceTypeId}&serviceRequestId=${serviceRequest.id}`}
                        legacyBehavior>
                        {getLocal("Edit")}
                      </Link>
                    ) : null}
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails style={{ display: "block" }}>
                {serviceRequest.responses && serviceRequestDocs ? (
                  <>
                    <ServiceRequestDetailTbl
                      requirements={serviceRequestDocs}
                      serviceType={serviceType}
                      serviceRequest={serviceRequest}
                      RequirementReceptForm={RequirementReceptForm}
                      onPrintInvoice={onPrintInvoice}
                      onPrintReceipt={onPrintReceipt}
                      SubmitDoc={SubmitDoc}
                      getAttachment={getAttachment}
                      getLocal={getLocal}
                      getLocalByVal={getLocalByVal}
                    />
                  </>
                ) : null}
              </AccordionDetails>
            </Accordion>
            {serviceType.allowOptionalEntries ? (
              <Accordion defaultExpanded={true}>
                <AccordionSummary
                  style={{ height: "1px" }}
                  expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
                >
                  <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    <p className="links">
                      {" "}
                      {getLocal("Optional")} {getLocal("Requirements")}{" "}
                    </p>
                  </div>
                </AccordionSummary>
                <AccordionDetails style={{ display: "block" }}>
                  {serviceType.optionalEntryLabels &&
                  serviceType.optionalEntryLabels.$values ? (
                    <>
                      <ServiceRequestOptionals
                        loadRequest={loadRequest}
                        serviceType={serviceType}
                        optionalEntries={
                          serviceRequest.optionalEntries.$values
                        }
                        optionalEntryLookupValues={
                          serviceType.optionalEntryLabels.$values
                        }
                        showNotification={showNotification}
                        setErrorMsg={setErrorMsg}
                        requestId={serviceRequest.id}
                        showUpdateButtons={
                          serviceRequest.rejectedDocumentsNo > 0 ||
                          serviceRequest.requestStatusValue === 0
                        }
                        getLocal={getLocal}
                        getLocalByVal={getLocalByVal}
                      />
                    </>
                  ) : null}
                </AccordionDetails>
              </Accordion>
            ) : null}
            <Accordion defaultExpanded={true}>
              <AccordionSummary
                style={{ height: "1px" }}
                expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
              >
                <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                  <p className="links"> {getLocal("Followups")} </p>
                </div>
              </AccordionSummary>
              <AccordionDetails>
                {serviceRequest.followups &&
                serviceRequest.followups.$values ? (
                  <>
                    <ServiceRequestFollowupTbl
                      followups={serviceRequest.followups.$values}
                      serviceType={serviceType}
                      getLocal={getLocal}
                      getLocalByVal={getLocalByVal}
                    />
                  </>
                ) : null}
              </AccordionDetails>
            </Accordion>
            {serviceType.allowAdditionalAttachments ? (
              <Accordion defaultExpanded={true}>
                <AccordionSummary
                  style={{ height: "1px" }}
                  expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
                >
                  <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    <p className="links">
                      {" "}
                      {getLocal("Additional")} {getLocal("Attachments")}{" "}
                    </p>
                  </div>
                </AccordionSummary>
                <AccordionDetails style={{ display: "block" }}>
                  {serviceRequest.attachments &&
                  serviceRequest.attachments.$values ? (
                    <>
                      <ServiceRequestAttachments
                        loadRequest={loadRequest}
                        serviceType={serviceType}
                        attachments={serviceRequest.attachments.$values}
                        showNotification={showNotification}
                        setErrorMsg={setErrorMsg}
                        requestId={serviceRequest.id}
                        showUpdateButtons={
                          serviceRequest.rejectedDocumentsNo > 0 ||
                          serviceRequest.requestStatusValue === 0
                        }
                        getLocal={getLocal}
                        getLocalByVal={getLocalByVal}
                      />
                    </>
                  ) : null}
                </AccordionDetails>
              </Accordion>
            ) : null}
          </Grid>
        </Grid>
        <ConfirmDialog
          open={openConfirm}
          setOpen={setOpenConfirm}
          btnValue={btnValue}
          text={dialogText}
          resultAction={resultAction}
          param={param}
          getLocal={getLocal}
          getLocalByVal={getLocalByVal}
        />
        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={errorMsg ? errorMsg : getLocal("Failed to Save")}
          open={error}
          onClose={() => setError(false)}
        />
      </>
    ) : null}
  </>;
}
