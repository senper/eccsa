import React from "react";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
  Snackbar,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHandshake } from "@fortawesome/free-solid-svg-icons";
import Link from "next/link";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import {
  deleteExportAgreement,
  getExportAgreements,
} from "../../api/exportAgreement";
import ConfirmDialog from "../Common/ConfirmDialog";
import { AddAlert } from "@mui/icons-material";
import { Card, Col, Form, InputGroup, Row } from "react-bootstrap";

export default function ExportAgreementsList({
  expanded = true,
  getLocal,
  getLocalByVal,
}) {
  const token = localStorage.getItem("access_token");
  const [filterStatus, setFilterStatus] = React.useState(1);
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
  const [list, setList] = React.useState([]);
  React.useEffect(() => {
    if (token) {
      loadList(filterStatus);
    }
  }, [token]);
  const loadList = (status) => {
    getExportAgreements(status).then((data) => {
      setList(data.data.$values);
    });
  };
  //
  const [openConfirm, setOpenConfirm] = React.useState(false);
  const [dialogText, setDialogText] = React.useState(<></>);
  const [resultAction, setResultAction] = React.useState(null);
  const [param, setParam] = React.useState(null);
  ///
  const Remove = (id, agreementNo) => {
    setOpenConfirm(true);
    setParam(id);
    setResultAction(() => ConfirmRemove);
    setDialogText(
      <>
        {getLocal("Remove Export Agreement with agreement No")}{" "}
        <strong>{agreementNo}</strong>.
      </>
    );
    const ConfirmRemove = (id) => {
      setOpenConfirm(false);
      deleteExportAgreement(id)
        .then((data) => {
          loadList(filterStatus);
        })
        .catch((error) => {
          // console.log(error);
          showNotification("tr");
        });
    };
  };
  const columns = [
    {
      headerName: getLocal("Agreement No"),
      field: "agreementNo",
      flex: 0.2,
      renderCell: (params) => (
        <Link
          href={`/myServices/exportAgreementDetailPage?agreementId=${params.row.id}`}
          legacyBehavior>
          {params.value}
        </Link>
      ),
    },
    {
      headerName: getLocal("License No"),
      field: "licenseNo",
      flex: 0.2,
    },
    {
      headerName: getLocal("Consignee"),
      field: "consignee",
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Country"),
      field: "country",
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("City"),
      field: "city",
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Status"),
      field: "status",
      flex: 0.15,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Actions"),
      field: "id",
      flex: 0.15,
      filterable: false,
      sortable: false,
      renderCell: (params) => (
        <div className="actions-right">
          <Link
            href={`/myServices/exportAgreementForm?id=${params.value}`}
            legacyBehavior>
            {getLocal("Edit")}
          </Link>
          {params.row.isLocked ? null : (
            <>
              &nbsp; &nbsp;
              <a
                className="text-danger"
                style={{ cursor: "pointer" }}
                onClick={() => Remove(params.value, params.row.agreementNo)}
              >
                {getLocal("Remove")}
              </a>
            </>
          )}
        </div>
      ),
    },
  ];
  return <>
    <Grid item xs={12}>
      <Accordion defaultExpanded={expanded}>
        <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
          <div className="icon">
            <FontAwesomeIcon icon={faHandshake} size="1x" />
          </div>

          <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
            <p className="links">
              {" "}
              {getLocal("Export Agreements")} (
              <Link href="/myServices/exportAgreementForm" legacyBehavior>
                {getLocal("Add New")}
              </Link>
              )
            </p>
          </div>
        </AccordionSummary>
        <AccordionDetails style={{ display: "block" }}>
          <Row>
            <Col md>
              <InputGroup>
                <InputGroup.Text>{getLocal("Filter Status")}</InputGroup.Text>
                <Form.Select
                  value={filterStatus}
                  onChange={(e) => {
                    setFilterStatus(e.target.value);
                    loadList(e.target.value);
                  }}
                >
                  <option value="1">Active</option>
                  <option value="0">Inactive</option>
                </Form.Select>
              </InputGroup>
            </Col>
            <Col md></Col>
          </Row>{" "}
          <>
            <div id="showOnLargeScreen">
              <CustomDataGrid dataRows={list} columns={columns} />
            </div>
            <div id="showOnSmallScreen">
              {list && list.length > 0 ? (
                list.map((r, i) => (
                  <Card key={i}>
                    <Card.Header style={{ color: "white" }}>
                      {r.agreementNo}
                    </Card.Header>
                    <Card.Body>
                      <Row>
                        <Col>{getLocal("Agreement No")}</Col>
                        <Col>
                          {" "}
                          <Link
                            href={`/myServices/exportAgreementDetailPage?agreementId=${r.id}`}
                            legacyBehavior>
                            {r.agreementNo}
                          </Link>
                        </Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("License No")}</Col>
                        <Col>{r.licenseNo}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Consignee")}</Col>
                        <Col>{r.consignee}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Country")}</Col>
                        <Col>{r.country}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("City")}</Col>
                        <Col>{r.city}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Status")}</Col>
                        <Col>{r.status}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Actions")}</Col>
                        <Col>
                          <div>
                            <Link href={`/myServices/exportAgreementForm?id=${r.id}`} legacyBehavior>
                              {getLocal("Edit")}
                            </Link>
                            {r.isLocked ? null : (
                              <>
                                &nbsp; &nbsp;
                                <a
                                  className="text-danger"
                                  style={{ cursor: "pointer" }}
                                  onClick={() => Remove(r.id, r.agreementNo)}
                                >
                                  {getLocal("Remove")}
                                </a>
                              </>
                            )}
                          </div>
                        </Col>
                      </Row>
                    </Card.Body>
                  </Card>
                ))
              ) : (
                <>No Export Agreement(s) Found</>
              )}
            </div>
          </>
        </AccordionDetails>
      </Accordion>
    </Grid>
    <ConfirmDialog
      open={openConfirm}
      setOpen={setOpenConfirm}
      btnValue={getLocal("Remove")}
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
  </>;
}
