import { AddAlert } from "@mui/icons-material";
import { Accordion, AccordionDetails, AccordionSummary, Grid, Snackbar } from "@mui/material";
import Link from "next/link";
import { useRouter } from "next/router";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import React from "react";
import {
  getAssociationForApply,
  getUserApplication,
  printApplicationInvoice,
  printApplicationReceipt,
  saveApplicationPayment,
  submitApplication,
  submitApplicationDocument,
} from "../../api/membership";
import ConfirmDialog from "../Common/ConfirmDialog";
import ApplicationDetailTbl from "./ApplicationDetailTbl";

export default function ApplicationDetail({ getLocal, getLocalByVal }) {
  const location = useRouter();
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
  const [dialogText, setDialogText] = React.useState(<></>);
  const [resultAction, setResultAction] = React.useState(null);
  const [param, setParam] = React.useState(null);
  ///
  let associationIdValue = 0;
  let requestIdValue = null;
  if (location.query) {
    const { associationId, applicationId } = location.query;
    associationIdValue = associationId;
    requestIdValue = applicationId;
  }
  ///
  const [association, setAssociation] = React.useState([]);
  React.useEffect(() => {
    if (associationIdValue) {
      getAssociationForApply(associationIdValue).then((data) => {
        setAssociation(data.data);
      });
    }
  }, [associationIdValue]);
  ///
  const [request, setRequest] = React.useState([]);
  const [requestDocs, setRequestDocs] = React.useState([]);
  function loadRequest() {
    getUserApplication(requestIdValue).then((data) => {
      setRequest(data.data);
      if (data.data.responses && data.data.responses.$values) {
        const typeValue = data.data.applicationTypeValue;
        const list = data.data.responses.$values
          .filter(
            (r) =>
              (r.requirement.requirementTypeValue != 2 &&
                typeValue == 0 &&
                r.requirement.applicationRequirementLevelValue != 0) ||
              (typeValue == 2 &&
                r.requirement.renewalRequirementLevelValue != 0)
          )
          .sort(compare);
        setRequestDocs(list);
      }
    });
  }
  React.useEffect(() => {
    if (requestIdValue) {
      loadRequest();
    }
  }, [requestIdValue]);
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
    if (!set.extension || set.extension.trim() == "") return set.receiptNo;
    const fileName = set.id.substr(set.id.length - 15).replace("-", "");
    const relativePath = `membership/documents/receipts/${fileName}${set.extension}`;

    return (
      <a
        href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
        target="_blank"
        rel="noreferrer"
        title={"Receipt " + set.receiptNo}
        download={"Receipt " + set.receiptNo}
      >
        {set.receiptNo}
      </a>
    );
  };

  function DisplayLastReceipt() {
    if (
      !request.invoice ||
      !request.invoice.settlments ||
      !request.invoice.settlments.$values
    ) {
      return null;
    }
    var settlment = request.invoice.settlments.$values.find(
      (s) => s.receiptNo == request.receiptNo
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
      !request.invoice ||
      !request.invoice.settlments ||
      !request.invoice.settlments.$values
    ) {
      return null;
    }
    var settlments = request.invoice.settlments.$values.filter(
      (s) => s.receiptNo != request.receiptNo
    );
    return (
      <>
        {settlments.length > 0 ? (
          <>
            <br />
            {getLocal("Other Receipts")}:{" "}
            {settlments.map((set) => (
              <>{set.extension ? getAttachment(set) : set.receiptNo} </>
            ))}
          </>
        ) : null}
      </>
    );
  }

  function ReceptForm() {
    if (
      !request.invoiceNo ||
      (request.receiptNo && request.paymentStatusInt == 0) ||
      request.paymentStatusInt == 1
    ) {
      return null;
    }
    return (
      <>
        <br />
        {getLocal("Receipt No")}:{" "}
        <input
          type="text"
          style={{ marginBottom: "10px", marginTop: "5px" }}
          onChange={(event) => setReceiptNo(event.target.value)}
        />
        <br />
        {getLocal("Receipt")}:{" "}
        <input
          type="file"
          disabled={paymentMethod != 1}
          required
          value={receiptAttachment ? receiptAttachment.fileName : ""}
          style={{ marginBottom: "10px", marginLeft: "20px" }}
          onChange={(event) => setReceiptAttachment(event.target.files[0])}
        />
        <br />
        {getLocal("Amount")}:{" "}
        <input
          type="number"
          disabled
          value={request.remainingAmount}
          min="0"
          step="any"
          style={{ marginLeft: "20px", marginBottom: "10px" }}
          onChange={(event) => setPaymentAmt(event.target.value)}
        />
        <button
          type="button"
          className="btn-sm btn btn-warning"
          style={{ float: "none", margin: "0px 0px 5px 5px" }}
          onClick={SavePayment}
        >
          {getLocal("Save")}
        </button>
      </>
    );
  }
  function SavePayment() {
    if (!receiptNo) {
      setErrorMsg(getLocal("Enter a valid Receipt No"));
      showNotification("tr");
      return;
    }
    setPaymentAmt(request.remainingAmount);
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
        <strong>{receiptNo}</strong> {getLocal("with amount")}{" "}
        <strong> {request.remainingAmount}</strong>
      </>
    );
  }
  function ConfirmSavePayment() {
    setOpenConfirm(false);
    var formData = new FormData();
    formData.append("applicationId", request.id);
    formData.append("receiptNo", receiptNo);
    formData.append("receipt", receiptAttachment);
    formData.append("paymentMethod", 1);
    formData.append("invoiceNo", request.invoiceNo);
    formData.append("amount", request.remainingAmount);
    const data = {
      requestId: request.id,
      receiptNo: receiptNo,
      paymentMethod: 1,
      invoiceNo: request.invoiceNo,
      amount: paymentAmt,
    };
    saveApplicationPayment(request.id, formData)
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
  function SubmitRequest() {
    setOpenConfirm(true);
    setResultAction(() => ConfirmSubmitRequest);
    setDialogText(
      <>
        {getLocal("Submit request")} {getLocal("with tracking No")}{" "}
        <strong>{request.trackingNo}</strong>
      </>
    );
  }
  function ConfirmSubmitRequest() {
    setOpenConfirm(false);
    submitApplication(request.id)
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
    setParam(res.id);
    setResultAction(() => ConfirmSubmitDoc);
    setDialogText(
      <>
        {getLocal("Submit response")} {getLocal("for requirement")}{" "}
        <strong>{res.requirement.name}</strong>
      </>
    );
  }
  function ConfirmSubmitDoc(id) {
    setOpenConfirm(false);
    submitApplicationDocument(id)
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
    if (request.applicationStatusValue === 0) {
      buttonText = getLocal("Submit");
      buttonAction = SubmitRequest;
      btnShow = true;
    } /* else if (request.requestStatusValue === 5) {
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
      request.extension &&
      (request.applicationStatusValue === 5 ||
        request.applicationStatusValue === 100)
    ) {
      const fileName = request.id
        .substr(request.id.length - 15)
        .replace("-", "");
      const relativePath = `membership/documents/${fileName}${request.extension}`;
      return (
        <>
          <br />
          <a
            href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
            target="_blank"
            rel="noreferrer"
          >
            {getLocal("Result Document")}
          </a>
        </>
      );
    }
  }
  function onPrintInvoice(id, invoiceNo) {
    printApplicationInvoice(id, invoiceNo)
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
          setErrorMsg(error.response.data);
          showNotification("tr");
        }
      });
  }
  function onPrintReceipt(id, invoiceNo, receiptNo) {
    printApplicationReceipt(id, invoiceNo)
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
          setErrorMsg(error.response.data);
          showNotification("tr");
        }
      });
  }
  function ShowInvoiceDownload() {
    if (!request.invoiceNo) {
      return null;
    }
    return (
      <a
        className="text-primary"
        style={{ cursor: "pointer" }}
        onClick={() => onPrintInvoice(request.id, request.invoiceNo)}
      >
        ({getLocal("Download Invoice")})
      </a>
    );
  }
  function ShowReceiptDownload() {
    if (!request.invoiceNo || !request.receiptNo || !request.printReceipt) {
      return null;
    }
    return (
      <Link
        onClick={() =>
          onPrintReceipt(request.id, request.invoiceNo, request.receiptNo)
        }
        to={{
          state: {
            applicationId: request.id,
            associationId: associationIdValue,
          },
        }}
        legacyBehavior>
        ({getLocal("Download Receipt")})
      </Link>
    );
  }
  return <>
    {association && request ? (
      <>
        <Grid container spacing={5}>
          <Grid item xs={12}>
            <Accordion defaultExpanded={true}>
              <AccordionSummary
                style={{ height: "1px" }}
                expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
              >
                <div className="text-xs font-weight-bold text-primary text-uppercase">
                  <p className="links">
                    {" "}
                    {getLocal("Application For")}{" "}
                    {getLocalByVal(
                      association.nameEng,
                      association.nameLocal
                    )}
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails>
                <Grid container spacing={5}>
                  <Grid item xs={12} sm={4}>
                    {getLocal("Application Date")}: {request.dateApplied}
                    <br />
                    {getLocal("Type")}: {request.applicationType}
                    <br />
                    {getLocal("Status")}: {request.applicationStatus}{" "}
                    {GetStatusChangeButton()}
                    {ShowResultDocument()}
                    <br />
                  </Grid>
                  <Grid item xs={12} sm={5}>
                    {getLocal("Invoice No")}: {request.invoiceNo}{" "}
                    {ShowInvoiceDownload()}
                    <br />
                    {getLocal("Last Receipt No")}: <>{DisplayLastReceipt()}</>{" "}
                    {request.paymentStatusInt == 3
                      ? "(" + request.paymentStatus + ")"
                      : null}
                    {ReceptForm()} {ShowReceiptDownload()}
                    <br />
                    {getLocal("Confirmed Amount")}:{" "}
                    {request.invoice ? request.invoice.totalPaid : 0}
                    {DisplayOtherReceipts()}
                    <br />
                  </Grid>
                  <Grid item xs={12} sm={3}>
                    <strong>{getLocal("Bank Accounts")}</strong>
                    <br />
                    {association.settings && association.settings.$values ? (
                      association.settings.$values.map((t, index) => (
                        <span key={index}>
                          {t.bankName}: {t.accountNumber}
                          <br />
                        </span>
                      ))
                    ) : (
                      <>{getLocal("Noting Found")}</>
                    )}
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
                    {getLocal("Documents")}{" "}
                    {request.rejectedDocumentsNo > 0 ||
                    request.applicationStatusValue === 0 ? (
                      <Link
                        href={`/myServices/membershipApplyForm?associationId=${request.associationId}&requestId=${request.id}&type=${request.applicationTypeValue}`}
                        legacyBehavior>
                        {getLocal("Edit")}
                      </Link>
                    ) : null}
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails style={{ display: "block" }}>
                {request.responses && requestDocs ? (
                  <>
                    <ApplicationDetailTbl
                      requirements={requestDocs}
                      association={association}
                      request={request}
                      onPrintInvoice={onPrintInvoice}
                      onPrintReceipt={onPrintReceipt}
                      SubmitDoc={SubmitDoc}
                      getLocal={getLocal}
                      getLocalByVal={getLocalByVal}
                    />
                  </>
                ) : null}
              </AccordionDetails>
            </Accordion>
          </Grid>
        </Grid>
        <ConfirmDialog
          open={openConfirm}
          setOpen={setOpenConfirm}
          btnValue={getLocal("Submit")}
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
