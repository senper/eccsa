import { AddAlert } from "@mui/icons-material";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
  Snackbar,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { useRouter } from "next/router";
import React from "react";
import { Card, Col, Row, Table } from "react-bootstrap";
import {
  closeCertOfOriginDocument,
  getUserCertificateOfOrigin,
} from "../../api/serviceRequest";
import ConfirmDialog from "../Common/ConfirmDialog";

export default function CertificateOfOriginDetail({ getLocal, getLocalByVal }) {
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
  ///
  const [openConfirm, setOpenConfirm] = React.useState(false);
  const [dialogText, setDialogText] = React.useState(<></>);
  const [resultAction, setResultAction] = React.useState(null);

  let certificateOfOriginIdValue = 0;
  if (location.query) {
    const { certificateOfOriginId } = location.query;
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
        {getLocal("Close request")} {getLocal("with No")}{" "}
        <strong>{certificateOfOrigin.apfRefNo}</strong>
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
        // console.log(error);
        showNotification("tr");
      });
  }
  return (
    <Grid container spacing={5}>
      {loading ? (
        <h2 className="section-title">
          <br />
          {getLocal("Loading")} {getLocal("certificate Of origin detail")}...
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
                <div className="text-xs font-weight-bold text-primary text-uppercase">
                  <p className="links">
                    {" "}
                    {getLocal("No")}:{" "}
                    <strong>{certificateOfOrigin.apfRefNo}</strong> <br />
                    <a
                      target={"_blank"}
                      href="https://esw.et/esw-trd/"
                      rel="noreferrer"
                    >
                      {getLocal("Official Url")}
                    </a>{" "}
                  </p>
                </div>
              </AccordionSummary>
              <AccordionDetails style={{ display: "block" }}>
                <Grid container spacing={0}>
                  <Grid item xs={12}>
                    {getLocal("Consignee")}:{" "}
                    <strong>{`${certificateOfOrigin.consigneeCompany}, 
                    ${certificateOfOrigin.consigneeName}`}</strong>
                    <br />
                    {getLocal("TIN")}:{" "}
                    <strong>{certificateOfOrigin.consigneeTin}</strong>
                    <br />
                    {getLocal("Address")}:{" "}
                    <strong>{certificateOfOrigin.consigneeAddress}</strong>
                    <br />
                    {getLocal("H.No")}:{" "}
                    <strong>{certificateOfOrigin.consigneeHouseNo}</strong>
                    <br />
                    {getLocal("Tel")}:{" "}
                    <strong>{certificateOfOrigin.consigneeTelNo}</strong>
                    <br />
                    {getLocal("P.O.Box")}:{" "}
                    <strong>{certificateOfOrigin.consigneePoBox}</strong>
                    <br />
                    {getLocal("Port of Destination")}:{" "}
                    <strong>{certificateOfOrigin.portOfDestinationName}</strong>
                    <br />
                  </Grid>
                  <Grid item xs={12}>
                    {getLocal("Consignor")}:{" "}
                    <strong>{`${certificateOfOrigin.consignorCompany}`}</strong>{" "}
                    <br />
                    {getLocal("Country")}:{" "}
                    <strong>{`${certificateOfOrigin.consignorCountryName}`}</strong>
                    <br />
                    {getLocal("Address")}:{" "}
                    <strong>{`${certificateOfOrigin.consignorAddress}`}</strong>
                    <br />
                    {getLocal("City")}:{" "}
                    <strong>
                      {certificateOfOrigin.consignorCityName
                        ? certificateOfOrigin.consignorCityName
                        : ""}
                    </strong>
                    <br />
                    {getLocal("Tel.")}:{" "}
                    <strong>{`${certificateOfOrigin.consignorTelephone}`}</strong>
                    <br />
                    {getLocal("Email")}:{" "}
                    <strong>{`${certificateOfOrigin.consignorEmail}`}</strong>
                    <br />
                    {getLocal("Type of Carriage")}:{" "}
                    <strong>{certificateOfOrigin.dptrTrmnCd}</strong>
                    <br />
                  </Grid>
                </Grid>
                <hr style={{ width: "100%", margin: "0", marginTop: "5px" }} />
                <Grid container spacing={0}>
                  <Grid item xs={12}>
                    {getLocal("Request Status")}:{" "}
                    <strong>{certificateOfOrigin.status}</strong> <b />
                    <b />
                    {certificateOfOrigin.status == "Accepted" ? (
                      <button
                        type="button"
                        className="btn-sm btn btn-warning "
                        style={{ float: "none", margin: "0" }}
                        onClick={() => CloseRequest()}
                      >
                        {getLocal("Close")}
                      </button>
                    ) : null}
                    <br />
                  </Grid>
                </Grid>
                <Grid container spacing={0}>
                  <Grid item xs={12}>
                    <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                      <p className="links"> {getLocal("Commodities")} </p>
                    </div>
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
                                <th style={{ width: "150px" }} rowSpan={2}>
                                  {getLocal("Marks")} {getLocal("&")}{" "}
                                  {getLocal("Number of Packages")}
                                </th>
                                <th rowSpan={2}>
                                  {getLocal("Quantity and Type of Packages")}
                                </th>
                                <th colSpan={2}>{getLocal("weight Kgs")}</th>
                                <th rowSpan={2}>{getLocal("HS Code")}</th>
                                <th rowSpan={2}>
                                  {getLocal("Description of Goods")}
                                </th>
                              </tr>
                              <tr>
                                <td>{getLocal("Gross")}</td>
                                <td>{getLocal("Net")}</td>
                              </tr>
                            </thead>
                            <tbody>
                              {commodities.length > 0 ? (
                                commodities.map((com, index) => (
                                  <tr key={index}>
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
                                  <td colSpan="6" className="text-center">
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
                                <Card.Header>{`${com.pkgCnt} ${com.pkgTpCd}`}</Card.Header>
                                <Card.Body>
                                  <Row>
                                    <Col>
                                      {getLocal("Marks")} {getLocal("&")}{" "}
                                      {getLocal("Number of Packages")}
                                    </Col>
                                    <Col>{`${com.pkgCnt} ${com.pkgTpCd}`}</Col>
                                  </Row>
                                  <Row>
                                    <Col>
                                      {getLocal(
                                        "Quantity and Type of Packages"
                                      )}
                                    </Col>
                                    <Col>{`${com.quantity} in ${com.quantityUnitCode}; ${com.pkgTpCd}`}</Col>
                                  </Row>
                                  <Row>
                                    <Col>{getLocal("weight Kgs Gross")} </Col>
                                    <Col>
                                      [{com.grossWeightCode}] {com.grossWeight}
                                    </Col>
                                  </Row>
                                  <Row>
                                    <Col>{getLocal("weight Kgs Net")} </Col>
                                    <Col>{com.netWeight}</Col>
                                  </Row>
                                  <Row>
                                    <Col>{getLocal("HS Code")}</Col>
                                    <Col>{com.hsCode}</Col>
                                  </Row>
                                  <Row>
                                    <Col>
                                      {getLocal("Description of Goods")}
                                    </Col>
                                    <Col>{com.goodsDescription}</Col>
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
                  </Grid>
                </Grid>
              </AccordionDetails>
            </Accordion>
          </Grid>
          <ConfirmDialog
            open={openConfirm}
            setOpen={setOpenConfirm}
            btnValue={getLocal("Submit")}
            text={dialogText}
            resultAction={resultAction}
            param={{}}
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
