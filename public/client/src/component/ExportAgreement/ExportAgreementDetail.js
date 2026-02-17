import React from "react";
import {
  Grid,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Snackbar,
} from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../ServiceRequest/ServiceRequestDetailTbl.css";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { AddAlert } from "@material-ui/icons";
import { useForm } from "react-hook-form";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import {
  getExportAgreement,
  saveCommodityList,
} from "../../services/exportAgreement";
import { useLocation } from "react-router-dom";
import { Table } from "react-bootstrap";

export default function ExportAgreementDetail() {
  const location = useLocation();
  const {
    register,
    setValue,
    getValues,
  } = useForm();
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
  if (location.state) {
    const { agreementId } = location.state;
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
      setErrorMsg("HS Code is required");
      showNotification("tr");
      return;
    } else if (!commodity.name) {
      setErrorMsg("Name is required");
      showNotification("tr");
      return;
    }
    if (commodities.find((v) => v.hsCode === commodity.hsCode)) {
      setErrorMsg("Commodity with HS Code already added");
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
          setErrorMsg("Commodity List Saved!");
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
        }
        showNotification("tr");
      })
      .catch((error) => {
        console.log(error.response);
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
    <Grid container spacing={5}>
      {agreement ? (
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
                    Export Agreement No:{" "}
                    <strong>{agreement.agreementNo}</strong>
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails>
                <Grid container spacing={5}>
                  <Grid item xs={6}>
                    Agreement No: <strong>{agreement.agreementNo}</strong>
                    <br />
                    Consignee: <strong>{agreement.consignee}</strong>
                    <br />
                    Country: <strong>{agreement.country}</strong>
                    <br />
                    County Of Destination:{" "}
                    <strong>{agreement.countyOfDestination}</strong>
                    <br />
                    Port Of Departure:{" "}
                    <strong>{agreement.portOfDeparture}</strong>
                    <br />
                    Term Of Delivery:{" "}
                    <strong>{agreement.termOfDelivery}</strong>
                    <br />
                    Status: <strong>{agreement.status}</strong>
                    <br />
                  </Grid>
                  <Grid item xs={6}>
                    License No: <strong>{agreement.licenseNo}</strong>
                    <br />
                    Type Of Carriage:{" "}
                    <strong>{agreement.typeOfCarriage}</strong>
                    <br />
                    City: <strong>{agreement.city}</strong>
                    <br />
                    Port Of Destination:{" "}
                    <strong>{agreement.portOfDestination}</strong>
                    <br />
                    Name Of Carrier: <strong>{agreement.nameOfCarrier}</strong>
                    <br />
                    Address: <strong>{agreement.address}</strong>
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
                    Commodities{" "}
                    {agreement && !agreement.isLocked ? (
                      <>
                        <button
                          type="button"
                          class="btn-sm btn btn-primary"
                          style={{ float: "none", margin: "1px 0px 0 10px" }}
                          onClick={(event) => {
                            event.stopPropagation();
                            selectCommodity({ ...emptyCommodity });
                          }}
                        >
                          Add
                        </button>
                        &nbsp; &nbsp;
                        <button
                          type="button"
                          class="btn-sm btn btn-warning"
                          style={{ float: "none", margin: "1px 0px 0 10px" }}
                          onClick={(event) => {
                            event.stopPropagation();
                            postCommodityList();
                          }}
                        >
                          Save
                        </button>
                      </>
                    ) : null}
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails style={{ display: "block" }}>
                {commodities ? (
                  <Table
                    striped
                    bordered
                    hover
                    size="sm"
                    className="three-dtbl"
                  >
                    <thead>
                      <tr>
                        <th style={{ width: "150px" }}>HSCode</th>
                        <th>Name</th>
                        <th style={{ width: "150px" }}>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      {commodities.length > 0 ? (
                        commodities.sort(compare).map((com) => (
                          <tr>
                            <td>{com.hsCode}</td>
                            <td>{com.name}</td>
                            <td>
                              <div className="actions-right">
                                {/*<button
                                  type="button"
                                  class="btn-sm btn btn-primary"
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
                                      class="btn-sm btn btn-warning"
                                      style={{
                                        float: "none",
                                        margin: "0px 0px 0 10px",
                                      }}
                                      onClick={() => removeCommodity(com)}
                                    >
                                      Remove
                                    </button>
                                  </>
                                )}
                              </div>
                            </td>
                          </tr>
                        ))
                      ) : (
                        <tr>
                          <td colspan="3" className="text-center">
                            <strong>Empty List</strong>
                          </td>
                        </tr>
                      )}
                    </tbody>
                  </Table>
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
              {(selectedCommodity ? "Add" : "Edit") + " Commodity"}
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
                HS Code:{" "}
                <input
                  type="text"
                  className="form-control"
                  {...register("hsCode", {
                    required: true,
                    maxLength: 50,
                  })}
                />
                <br />
                Name:{" "}
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
                class="btn btn-sm btn-outline-info"
                onClick={() => setShowEditDialog(false)}
                style={{ float: "none", margin: "0 10px 0 0" }}
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn-sm btn btn-warning"
                style={{ float: "none", margin: "5px 0px 0 10px" }}
                onClick={() => addCommodity()}
              >
                Add
              </button>
            </DialogActions>
          </Dialog>
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
  );
}

// export default App;
