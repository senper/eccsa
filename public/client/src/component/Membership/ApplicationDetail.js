import React from "react";
import {
  Grid,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Snackbar,
  Breadcrumbs,
} from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import { Link } from "react-router-dom";
import "../../App.css";
import { useLocation } from "react-router-dom";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import ConfirmDialog from "../Services/ConfirmDialog";
import { AddAlert } from "@material-ui/icons";
import {
  getAssociationForApply,
  getUserApplication,
  printApplicationInvoice,
  printApplicationReceipt,
  saveApplicationPayment,
  submitApplication,
  submitApplicationDocument,
} from "../../services/membership";
import ApplicationDetailTbl from "./ApplicationDetailTbl";
import { makeStyles } from "@material-ui/core/styles";
import Footer from "../Footer/Footer";

const useStyle = makeStyles((theme) => ({
  breadcrum: {
    padding: "10px",
    marginLeft: "100px",
  },
}));

export default function ApplicationDetail() {
  const classes = useStyle();
  const location = useLocation();
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
  if (location.state) {
    const { associationId, applicationId } = location.state;
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
            Other Receipts:{" "}
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
        Receipt No:{" "}
        <input
          type="text"
          style={{ marginBottom: "10px", marginTop: "5px" }}
          onChange={(event) => setReceiptNo(event.target.value)}
        />
        <br />
        Receipt:{" "}
        <input
          type="file"
          disabled={paymentMethod != 1}
          required
          value={receiptAttachment ? receiptAttachment.fileName : ""}
          style={{ marginBottom: "10px", marginLeft: "20px" }}
          onChange={(event) => setReceiptAttachment(event.target.files[0])}
        />
        <br />
        Amount:{" "}
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
          class="btn-sm btn btn-warning"
          style={{ float: "none", margin: "0px 0px 5px 5px" }}
          onClick={SavePayment}
        >
          Save
        </button>
      </>
    );
  }
  function SavePayment() {
    if (!receiptNo) {
      setErrorMsg("Enter a valid Receipt No");
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
        Save Payment with receipt No <strong>{receiptNo}</strong> with amount{" "}
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
        console.log(error.response);
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
        Submit request with tracking No <strong>{request.trackingNo}</strong>
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
        console.log(error);
        showNotification("tr");
      });
  }
  function SubmitDoc(res) {
    setOpenConfirm(true);
    setParam(res.id);
    setResultAction(() => ConfirmSubmitDoc);
    setDialogText(
      <>
        Submit response for requirement <strong>{res.requirement.name}</strong>
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
        console.log(error);
        showNotification("tr");
      });
  }
  function GetStatusChangeButton() {
    let buttonText = "";
    let buttonAction = null;
    let btnShow = false;
    if (request.applicationStatusValue === 0) {
      buttonText = "Submit";
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
          class="btn-sm btn btn-warning "
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
            Result Document
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
        console.log(error.response);
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
        console.log(error.response);
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
      <Link
        onClick={() => onPrintInvoice(request.id, request.invoiceNo)}
        to={{
          state: {
            applicationId: request.id,
            associationId: associationIdValue,
          },
        }}
      >
        (Download Invoice)
      </Link>
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
      >
        (Download Receipt)
      </Link>
    );
  }
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid container spacing={2} className="grid">
          <Grid item xs={12} md={12}>
            <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
              <Link color="inherit" to="/">
                Home
              </Link>

              <Link
                color="textPrimary"
                as={Link}
                to={{
                  pathname: "/myServices",
                  state: {
                    eventKey: "myServices",
                    navTo: "membership",
                  },
                }}
                aria-current="page"
              >
                Membership
              </Link>
            </Breadcrumbs>
          </Grid>
        </Grid>
        <Grid item xs={1} md={1}>
          {/* <Welcome /> */}
        </Grid>
        <Grid item xs={10} md={10}>
          <Grid container spacing={5}>
            {association && request ? (
              <>
                <Grid item xs={12}>
                  <Accordion defaultExpanded={true}>
                    <AccordionSummary
                      style={{ height: "1px" }}
                      expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
                    >
                      <div class="text-xs font-weight-bold text-primary text-uppercase">
                        <p class="links">
                          {" "}
                          Application For {association.nameEng}
                        </p>
                      </div>
                    </AccordionSummary>
                    <AccordionDetails>
                      <Grid container spacing={5}>
                        <Grid item xs={12} sm={4}>
                          Application Date: {request.dateApplied}
                          <br />
                          Type: {request.applicationType}
                          <br />
                          Status: {request.applicationStatus}{" "}
                          {GetStatusChangeButton()}
                          {ShowResultDocument()}
                          <br />
                        </Grid>
                        <Grid item xs={12} sm={5}>
                          Invoice No: {request.invoiceNo}{" "}
                          {ShowInvoiceDownload()}
                          <br />
                          Last Receipt No: <>{DisplayLastReceipt()}</>{" "}
                          {request.paymentStatusInt == 3
                            ? "(" + request.paymentStatus + ")"
                            : null}
                          {ReceptForm()} {ShowReceiptDownload()}
                          <br />
                          Confirmed Amount:{" "}
                          {request.invoice ? request.invoice.totalPaid : 0}
                          {DisplayOtherReceipts()}
                          <br />
                        </Grid>
                        <Grid item xs={12} sm={3}>
                          <strong>Bank Accounts</strong>
                          <br />
                          { association.settings && association.settings.$values  
                           ? 
                           association.settings.$values.map(t => (
                             <>{t.bankName}: {t.accountNumber}<br/></>
                           ))
                           : <>Noting Found</>}
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
                      <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                        <p class="links">
                          {" "}
                          Documents{" "}
                          {request.rejectedDocumentsNo > 0 ||
                          request.applicationStatusValue === 0 ? (
                            <Link
                              to={{
                                pathname: "/membershipApplicationForm",
                                state: {
                                  associationId: request.associationId,
                                  requestId: request.id,
                                  type: request.applicationTypeValue,
                                },
                              }}
                            >
                              {" "}
                              Edit
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
                          />
                        </>
                      ) : null}
                    </AccordionDetails>
                  </Accordion>
                  {/*<Accordion defaultExpanded={true}>
              <AccordionSummary
                style={{ height: "1px" }}
                expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
              >
                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                  <p class="links"> Followups </p>
                </div>
              </AccordionSummary>
              <AccordionDetails>
                {request.followups && request.followups.$values ? (
                  <>
                     <ServiceRequestFollowupTbl
                      followups={request.followups.$values}
                      association={association}
                    /> 
                  </>
                ) : null}
              </AccordionDetails>
            </Accordion>*/}
                  {/* {association.allowAdditionalAttachments ? (
              <Accordion defaultExpanded={true}>
                <AccordionSummary
                  style={{ height: "1px" }}
                  expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
                >
                  <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    <p class="links"> Additional Attachments </p>
                  </div>
                </AccordionSummary>
                <AccordionDetails style={{ display: "block" }}>
                  {request.attachments && request.attachments.$values ? (
                    <>
                       <ServiceRequestAttachments
                        loadRequest={loadRequest}
                        association={association}
                        attachments={request.attachments.$values}
                        showNotification={showNotification}
                        setErrorMsg={setErrorMsg}
                        requestId={request.id}
                        showUpdateButtons={
                          request.rejectedDocumentsNo > 0 ||
                          request.requestStatusValue === 0
                        }
                      /> 
                    </>
                  ) : null}
                </AccordionDetails>
              </Accordion>
            ) : null}*/}
                </Grid>
                <ConfirmDialog
                  open={openConfirm}
                  setOpen={setOpenConfirm}
                  btnValue={"Submit"}
                  text={dialogText}
                  resultAction={resultAction}
                  param={param}
                />
                <Snackbar
                  place="tr"
                  color="danger"
                  icon={AddAlert}
                  message={errorMsg ? errorMsg : "Failed to Save"}
                  open={error}
                  closeNotification={() => setError(false)}
                  close
                />
              </>
            ) : null}
          </Grid>
        </Grid>
        <Grid item xs={1} md={1}></Grid>
      </Grid>
    </div>
  );
}

// export default App;
