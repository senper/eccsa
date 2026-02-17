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
import { useLocation } from "react-router-dom";
import { Table } from "react-bootstrap";
import {
  closeCertOfOriginDocument,
  getUserCertificateOfOrigin,
} from "../../services/serviceRequest";
import ConfirmDialog from "../Services/ConfirmDialog";

export default function CertificateOfOriginDetail() {
  const location = useLocation();
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
  ///
  const [openConfirm, setOpenConfirm] = React.useState(false);
  const [dialogText, setDialogText] = React.useState(<></>);
  const [resultAction, setResultAction] = React.useState(null);

  let certificateOfOriginIdValue = 0;
  if (location.state) {
    const { certificateOfOriginId } = location.state;
    certificateOfOriginIdValue = certificateOfOriginId;
  }
  ///
  const [certificateOfOrigin, setCertificateOfOrigin] = React.useState(null);
  const [commodities, setCommodities] = React.useState([]);
  React.useEffect(() => {
    if (certificateOfOriginIdValue) {
      loadRequest();
    }
  }, [certificateOfOriginIdValue]);
  ///
  function loadRequest() {
    setLoading(true);
    getUserCertificateOfOrigin(certificateOfOriginIdValue)
      .then((data) => {
        setCertificateOfOrigin(data.data);
        if (data.data && data.data.commodityList.$values) {
          setCommodities(data.data.commodityList.$values);
        }
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }
  function CloseRequest() {
    setOpenConfirm(true);
    setResultAction(() => ConfirmCloseRequest);
    setDialogText(
      <>
        Close request with No <strong>{certificateOfOrigin.apfRefNo}</strong>
      </>
    );
  }
  function ConfirmCloseRequest() {
    setOpenConfirm(false);
    closeCertOfOriginDocument(certificateOfOriginIdValue)
      .then((data) => {
        loadRequest();
      })
      .catch((error) => {
        console.log(error);
        showNotification("tr");
      });
  }
  return (
    <Grid container spacing={5}>
      {loading ? (
        <h2 className="section-title">
          <br />
          Loading certificate Of origin detail...
        </h2>
      ) : null}
      {certificateOfOrigin ? (
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
                    No: <strong>{certificateOfOrigin.apfRefNo}</strong> <br />
                    <a target={"_blank"} href="https://esw.et/esw-trd/">
                      Official Url
                    </a>{" "}
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails style={{ display: "block" }}>
                <Grid container spacing={0}>
                  <Grid item xs={6}>
                    Consignee:{" "}
                    <strong>{`${certificateOfOrigin.consigneeCompany}, 
                    ${certificateOfOrigin.consigneeName}`}</strong>
                    <br />
                    TIN: <strong>{certificateOfOrigin.consigneeTin}</strong>
                    <br />
                    Address:{" "}
                    <strong>{certificateOfOrigin.consigneeAddress}</strong>
                    <br />
                    H.No:{" "}
                    <strong>{certificateOfOrigin.consigneeHouseNo}</strong>
                    <br />
                    Tel: <strong>{certificateOfOrigin.consigneeTelNo}</strong>
                    <br />
                    P.O.Box:{" "}
                    <strong>{certificateOfOrigin.consigneePoBox}</strong>
                    <br />
                    Port of Destination:{" "}
                    <strong>{certificateOfOrigin.portOfDestinationName}</strong>
                    <br />
                  </Grid>
                  <Grid item xs={6}>
                    Consignor:{" "}
                    <strong>{`${certificateOfOrigin.consignorCompany}`}</strong>{" "}
                    <br />
                    Country:{" "}
                    <strong>{`${certificateOfOrigin.consignorCountryName}`}</strong>
                    <br />
                    Address:{" "}
                    <strong>{`${certificateOfOrigin.consignorAddress}`}</strong>
                    <br />
                    City:{" "}
                    <strong>
                      {certificateOfOrigin.consignorCityName
                        ? certificateOfOrigin.consignorCityName
                        : ""}
                    </strong>
                    <br />
                    Tel.:{" "}
                    <strong>{`${certificateOfOrigin.consignorTelephone}`}</strong>
                    <br />
                    Email:{" "}
                    <strong>{`${certificateOfOrigin.consignorEmail}`}</strong>
                    <br />
                    Type of Carriage:{" "}
                    <strong>{certificateOfOrigin.dptrTrmnCd}</strong>
                    <br />
                  </Grid>
                </Grid>
                <hr style={{ width: "100%", margin: "0", marginTop: "5px" }} />
                <Grid container spacing={0}>
                  <Grid item xs={12}>
                    Request Status:{" "}
                    <strong>{certificateOfOrigin.status}</strong> <b />
                    <b />
                    {certificateOfOrigin.status == "Accepted" ? (
                      <button
                        type="button"
                        class="btn-sm btn btn-warning "
                        style={{ float: "none", margin: "0" }}
                        onClick={() => CloseRequest()}
                      >
                        Close
                      </button>
                    ) : null}
                    <br />
                  </Grid>
                </Grid>
                <Grid container spacing={0}>
                  <Grid item xs={12}>
                    <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                      <p class="links"> Commodities </p>
                    </div>
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
                            <th style={{ width: "150px" }} rowSpan={2}>
                              Marks & Number of Packages
                            </th>
                            <th rowSpan={2}>Quantity and Type of Packages</th>
                            <th colSpan={2}>weight Kgs</th>
                            <th rowSpan={2}>HS Code</th>
                            <th rowSpan={2}>Description of Goods</th>
                          </tr>
                          <tr>
                            <td>Gross</td>
                            <td>Net</td>
                          </tr>
                        </thead>
                        <tbody>
                          {commodities.length > 0 ? (
                            commodities.map((com) => (
                              <tr>
                                <td>{`${com.pkgCnt} ${com.pkgTpCd}`}</td>
                                <td>{`${com.quantity} in ${com.quantityUnitCode}; ${com.pkgTpCd}`}</td>
                                <td>
                                  [{com.grossWeightCode}] {com.grossWeight}
                                </td>
                                <td>{com.netWeight}</td>
                                <td>{com.hsCode}</td>
                                <td>{com.goodsDescription}</td>
                              </tr>
                            ))
                          ) : (
                            <tr>
                              <td colspan="6" className="text-center">
                                <strong>Empty List</strong>
                              </td>
                            </tr>
                          )}
                        </tbody>
                      </Table>
                    ) : null}
                  </Grid>
                </Grid>
              </AccordionDetails>
            </Accordion>
          </Grid>
          <ConfirmDialog
            open={openConfirm}
            setOpen={setOpenConfirm}
            btnValue={"Submit"}
            text={dialogText}
            resultAction={resultAction}
            param={{}}
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
  );
}
