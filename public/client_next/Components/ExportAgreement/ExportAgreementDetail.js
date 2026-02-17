import React from "react";
import { useForm } from "react-hook-form";
import {
  getExportAgreement,
  saveCommodityList,
} from "../../api/exportAgreement";
import { Card, Col, Row, Table } from "react-bootstrap";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
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

export default function ExportAgreementDetail({ getLocal, getLocalByVal }) {
  const location = useRouter();
  const { register, setValue, getValues } = useForm();
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
  ///
  let agreementIdValue = 0;
  if (location.query) {
    const { agreementId } = location.query;
    agreementIdValue = agreementId;
  }
  ///
  const [showEditDialog, setShowEditDialog] = React.useState(false);
  const [agreement, setAgreement] = React.useState(null);
  const [commodities, setCommodities] = React.useState([]);
  const [selectedCommodity, setSelectedCommodity] = React.useState(null);
  React.useEffect(() => {
    if (agreementIdValue) {
      getExportAgreement(agreementIdValue).then((data) => {
        setAgreement(data.data);
        if (data.data && data.data.commodities.$values) {
          const list = [];
          data.data.commodities.$values.forEach((v) => {
            list.push({ id: v.id, hsCode: v.hsCode, name: v.name });
          });
          setCommodities(list);
        }
      });
    }
  }, [agreementIdValue]);
  ///
  const emptyCommodity = {
    id: "00000000-0000-0000-0000-000000000000",
    hsCode: "",
    name: "",
  };
  const addCommodity = () => {
    const values = getValues();
    const commodity = { ...emptyCommodity, agreementId: agreement.id };
    for (const [key, value] of Object.entries(values)) {
      commodity[key] = value;
    }
    if (!commodity.hsCode) {
      setErrorMsg(getLocal("HS Code is required"));
      showNotification("tr");
      return;
    } else if (!commodity.name) {
      setErrorMsg(getLocal("Name is required"));
      showNotification("tr");
      return;
    }
    if (commodities.find((v) => v.hsCode === commodity.hsCode)) {
      setErrorMsg(getLocal("Commodity with HS Code already added"));
      showNotification("tr");
      return;
    }
    const newList = [...commodities];
    newList.push(commodity);
    setCommodities(newList);
    setSelectedCommodity(emptyCommodity);
    setShowEditDialog(false);
  };
  //
  const selectCommodity = (commodity) => {
    for (const [key, value] of Object.entries(commodity)) {
      setValue(key, value);
    }
    setSelectedCommodity(commodity);
    setShowEditDialog(true);
  };
  const removeCommodity = (commodity) => {
    const index = commodities.findIndex((v) => v.hsCode === commodity.hsCode);
    const newList = [...commodities];
    newList.splice(index, 1);
    setCommodities(newList);
  };
  //
  const postCommodityList = () => {
    saveCommodityList(agreement.id, commodities)
      .then((data) => {
        if (data.data.saved) {
          setErrorMsg(getLocal("Commodity List Saved!"));
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
        }
        showNotification("tr");
      })
      .catch((error) => {
        // console.log(error.response);
        showNotification("tr");
      });
  };
  function compare(a, b) {
    if (a.hsCode < b.hsCode) {
      return -1;
    }
    if (a.hsCode > b.hsCode) {
      return 1;
    }
    return 0;
  }
  //
  return (
    <Grid container>
      {agreement ? (
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
                    {getLocal("Export Agreement No")}:{" "}
                    <strong>{agreement.agreementNo}</strong>
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails>
                <Grid container spacing={5}>
                  <Grid item xs={12}>
                    {getLocal("Agreement No")}:{" "}
                    <strong>{agreement.agreementNo}</strong>
                    <br />
                    {getLocal("Consignee")}:{" "}
                    <strong>{agreement.consignee}</strong>
                    <br />
                    {getLocal("Country")}: <strong>{agreement.country}</strong>
                    <br />
                    {getLocal("County Of Destination")}:{" "}
                    <strong>{agreement.countyOfDestination}</strong>
                    <br />
                    {getLocal("Port Of Departure")}:{" "}
                    <strong>{agreement.portOfDeparture}</strong>
                    <br />
                    {getLocal("Term Of Delivery")}:{" "}
                    <strong>{agreement.termOfDelivery}</strong>
                    <br />
                    {getLocal("Status")}: <strong>{agreement.status}</strong>
                    <br />
                  </Grid>
                  <Grid item xs={12}>
                    {getLocal("License No")}:{" "}
                    <strong>{agreement.licenseNo}</strong>
                    <br />
                    {getLocal("Type Of Carriage")}:{" "}
                    <strong>{agreement.typeOfCarriage}</strong>
                    <br />
                    {getLocal("City")}: <strong>{agreement.city}</strong>
                    <br />
                    {getLocal("Port Of Destination")}:{" "}
                    <strong>{agreement.portOfDestination}</strong>
                    <br />
                    {getLocal("Name Of Carrier")}:{" "}
                    <strong>{agreement.nameOfCarrier}</strong>
                    <br />
                    {getLocal("Address")}: <strong>{agreement.address}</strong>
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
                    {getLocal("Commodities")}{" "}
                    {agreement && !agreement.isLocked ? (
                      <>
                        <button
                          type="button"
                          className="btn-sm btn btn-primary"
                          style={{ float: "none", margin: "1px 0px 0 10px" }}
                          onClick={(event) => {
                            event.stopPropagation();
                            selectCommodity({ ...emptyCommodity });
                          }}
                        >
                          {getLocal("Add")}
                        </button>
                        &nbsp; &nbsp;
                        <button
                          type="button"
                          className="btn-sm btn btn-warning"
                          style={{ float: "none", margin: "1px 0px 0 10px" }}
                          onClick={(event) => {
                            event.stopPropagation();
                            postCommodityList();
                          }}
                        >
                          {getLocal("Save")}
                        </button>
                      </>
                    ) : null}
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails style={{ display: "block" }}>
                {commodities ? (
                  <>
                    <div id="showOnLargeScreen">
                      <Table
                        striped
                        bordered
                        hover
                        size="sm"
                        className="three-dtbl"
                      >
                        <thead>
                          <tr>
                            <th style={{ width: "150px" }}>
                              {getLocal("HSCode")}
                            </th>
                            <th>{getLocal("Name")}</th>
                            <th style={{ width: "150px" }}>
                              {getLocal("Actions")}
                            </th>
                          </tr>
                        </thead>
                        <tbody>
                          {commodities.length > 0 ? (
                            commodities.sort(compare).map((com, index) => (
                              <tr key={index}>
                                <td>{com.hsCode}</td>
                                <td>{com.name}</td>
                                <td>
                                  <div className="actions-right">
                                    {/*<button
                                  type="button"
                                  className="btn-sm btn btn-primary"
                                  style={{
                                    float: "none",
                                    margin: "0px 0px 0 10px",
                                  }}
                                  onClick={() => selectCommodity(com)}
                                >
                                  Edit
                                </button> */}
                                    {agreement.isLocked ? null : (
                                      <>
                                        &nbsp; &nbsp;
                                        <button
                                          type="button"
                                          className="btn-sm btn btn-warning"
                                          style={{
                                            float: "none",
                                            margin: "0px 0px 0 10px",
                                          }}
                                          onClick={() => removeCommodity(com)}
                                        >
                                          {getLocal("Remove")}
                                        </button>
                                      </>
                                    )}
                                  </div>
                                </td>
                              </tr>
                            ))
                          ) : (
                            <tr>
                              <td colSpan="3" className="text-center">
                                <strong>{getLocal("Empty List")}</strong>
                              </td>
                            </tr>
                          )}
                        </tbody>
                      </Table>
                    </div>
                    <div id="showOnSmallScreen">
                      {commodities && commodities.length > 0 ? (
                        commodities.map((com, i) => (
                          <Card key={i} className="card__margin-bottom">
                            <Card.Header>{com.name}</Card.Header>
                            <Card.Body>
                              <Row>
                                <Col>{getLocal("HSCode")}</Col>
                                <Col>{com.hsCode}</Col>
                              </Row>
                              <Row>
                                <Col>{getLocal("HSCode")}</Col>
                                <Col>{com.name}</Col>
                              </Row>
                              <Row>
                                <Col>{getLocal("Actions")}</Col>
                                <Col>
                                  {agreement.isLocked ? null : (
                                      <button
                                        type="button"
                                        className="btn-sm btn btn-warning"
                                        style={{
                                          float: "none",
                                          margin: "0px 0px 0 0px",
                                        }}
                                        onClick={() => removeCommodity(com)}
                                      >
                                        {getLocal("Remove")}
                                      </button>
                                  )}
                                </Col>
                              </Row>
                            </Card.Body>
                          </Card>
                        ))
                      ) : (
                        <Card className="card__margin-bottom">
                        <strong>{getLocal("Empty List")}</strong>
                        </Card>
                      )}
                    </div>
                  </>
                ) : null}
              </AccordionDetails>
            </Accordion>
          </Grid>
          <Dialog
            open={showEditDialog}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
          >
            <DialogTitle id="alert-dialog-title" className="alert-info">
              {(selectedCommodity ? getLocal("Add") : getLocal("Edit")) +
                ` ${getLocal("Commodity")}`}
            </DialogTitle>
            <DialogContent>
              <DialogContentText id="alert-dialog-description">
                <input
                  type="hidden"
                  {...register("id", {
                    required: true,
                    maxLength: 50,
                  })}
                />
                {getLocal("HS Code")}:{" "}
                <input
                  type="text"
                  className="form-control"
                  {...register("hsCode", {
                    required: true,
                    maxLength: 50,
                  })}
                />
                <br />
                {getLocal("Name")}:{" "}
                <input
                  type="text"
                  className="form-control"
                  {...register("name", {
                    required: true,
                    maxLength: 255,
                  })}
                />
              </DialogContentText>
            </DialogContent>
            <DialogActions>
              <button
                type="button"
                className="btn btn-sm btn-outline-info"
                onClick={() => setShowEditDialog(false)}
                style={{ float: "none", margin: "0 10px 0 0" }}
              >
                {getLocal("Cancel")}
              </button>
              <button
                type="button"
                className="btn-sm btn btn-warning"
                style={{ float: "none", margin: "5px 0px 0 10px" }}
                onClick={() => addCommodity()}
              >
                {getLocal("Add")}
              </button>
            </DialogActions>
          </Dialog>
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
