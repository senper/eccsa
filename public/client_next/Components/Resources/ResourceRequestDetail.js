import React from "react";
import { getServiceType } from "../../pages/api/servicetype";
import {
  getServiceRequest,
  submitRequest,
  closeDocument,
  savePayment,
  printInvoice,
  printReceipt,
  saveTeleOutTradeNo,
  downloadResource,
  getResourceUrl,
} from "../../api/serviceRequest";
import { initiatePay } from "../../api/telepay";
import { useRouter } from "next/router";
import { AddAlert, Link } from "@mui/icons-material";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
  Snackbar,
} from "@mui/material";
import ResourceCard from "./ResourceCard";
import ConfirmDialog from "../Common/ConfirmDialog";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

export default function ResourceRequestDetail({ getLocal, getLocalByVal }) {
  const location = useRouter();
  const [loading, setLoading] = React.useState(false);
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
  //// console.log(serviceRequestIdValue, serviceTypeIdValue);
  const [serviceRequest, setServiceRequest] = React.useState(null);
  function loadRequest() {
    setLoading(true);
    getServiceRequest(serviceRequestIdValue)
      .then((data) => {
        setServiceRequest(data.data);
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
          name="paymentMethod"
          checked="checked"
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
          title={getLocal(
            "Includes Internet Banking, Bank Transfer, Cash Payment"
          )}
        >
          1, {getLocal("Other")}
        </span>{" "}
        {"  "}
        <input
          type="radio"
          name="paymentMethod"
          disabled="disabled"
          style={{ marginBottom: "10px", marginLeft: "10px" }}
          onChange={(event) => {
            setPaymentMethod(2);
            setReceiptNo("");
            setReceiptAttachment(null);
          }}
        />
        {"  "}
        2, <img src="/images/telebirr2.jpg" width={"75px"} alt="telebirr"/>
        {"  "}
        <button
          type="button"
          className="btn-sm btn btn-warning"
          style={{ float: "none", margin: "5px 0px 0 10px" }}
          onClick={SavePayment}
          disabled={savingOutTradeNo}
        >
          {savingOutTradeNo ? getLocal("Saving") : getLocal("Save")}
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
            console.error(res.data.message);
            setErrorMsg(getLocal("telebirr Payment Start Failed!"));
            showNotification("tr");
            setSavingOutTradeNo(false);
          }
        })
        .catch((error) => {
          console.error(error);
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
        <strong>{receiptNo}</strong> & {getLocal("payment method")}
        <strong>
          {" "}
          {paymentMethod == 1 ? getLocal("Other") : getLocal("telebirr")}
        </strong>{" "}
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
    setButtonValue(getLocal("Close"));
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

  function GetStatusChangeButton() {
    let buttonText = "";
    let buttonAction = null;
    let btnShow = false;
    if (
      serviceRequest.requestStatusValue === 0 &&
      (serviceType.rateBasis !== "Flat" ||
        (serviceRequest.invoiceNo && serviceRequest.receiptNo))
    ) {
      buttonText = getLocal("Submit");
      buttonAction = SubmitRequest;
      btnShow = true;
    } else if (
      serviceRequest.requestStatusValue === 5 ||
      serviceRequest.requestStatusValue === 6
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
      serviceType.serviceOwnerStr &&
      serviceRequest.resource &&
      (serviceRequest.requestStatusValue === 5 ||
        serviceRequest.requestStatusValue === 100)
    ) {
      return (
        <>
          <br />{" "}
          <Link onClick={() => onGetDocument(serviceRequest.id)}>
            {getLocal("Download Resource")}
          </Link>
          {serviceRequest.resource.hasUrlForUserRequest ? (
            <>
              <br />{" "}
              <Link onClick={() => onGetUrl(serviceRequest.id)}>
                {getLocal("Get Url")}
              </Link>
            </>
          ) : null}
        </>
      );
    }
  }

  function onGetDocument(id) {
    downloadResource(id)
      .then((data) => {
        const url = window.URL.createObjectURL(new Blob([data.data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", `${serviceRequest.resource.title}.pdf`); //or any other extension
        document.body.appendChild(link);
        link.click();
        link.remove();
      })
      .catch((error) => {
        // console.log(error);
        if (!error.response) {
          setErrorMsg(getLocal("Failed To Download"));
        } else {
          setErrorMsg(error.response);
        }
        showNotification("tr");
      });
  }

  function onGetUrl(id) {
    getResourceUrl(id)
      .then((data) => {
        //const url = window.URL.createObjectURL(new Blob([data.data]));
        const link = document.createElement("a");
        link.href = data.data;
        link.setAttribute("target", `_blank`); //or any other extension
        document.body.appendChild(link);
        link.click();
        link.remove();
      })
      .catch((error) => {
        // console.log(error);
        // if (error.response) {
        setErrorMsg(getLocal("Failed To Open Url"));
        showNotification("tr");
        // }
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
  return (
    <Grid container >
      {loading ? (
        <h2 className="section-title">
          <br />
          {getLocal("Loading")} {getLocal("Request detail")}...
        </h2>
      ) : null}
      {serviceType && serviceRequest ? (
        <>
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
                    {getLocalByVal(serviceType.nameEng, serviceType.nameLocal)}
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails>
                <Grid container spacing={5}>
                  <Grid item xs={12} sm={4}>
                    {getLocal("Request Date")}: {serviceRequest.requestDateStr}
                    <br />
                    {getLocal("Tracking Number")}: {serviceRequest.trackingNo}
                    <br />
                    {getLocal("Status")}: {serviceRequest.requestStatus}{" "}
                    {GetStatusChangeButton()}
                    {ShowResultDocument()}
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
          <ResourceCard
            resource={serviceRequest.resource}
            showRequest={false}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
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
    </Grid>
  );
}
