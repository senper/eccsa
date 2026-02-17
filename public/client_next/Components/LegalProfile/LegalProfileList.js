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
import { faFileContract } from "@fortawesome/free-solid-svg-icons";
import Link from "next/link";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import {
  deleteLegalProfile,
  getLegalProfiles,
  LegalDocumentTypes,
} from "../../api/legalProfile";
import ConfirmDialog from "../Common/ConfirmDialog";
import { Card, Col, Form, InputGroup, Row } from "react-bootstrap";
import { AddAlert } from "@mui/icons-material";

export default function LegalProfilesList({
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
  }, []);
  const loadList = (status) => {
    getLegalProfiles(status).then((data) => {
      setList(data.data.$values);
    });
  };
  //
  const [openConfirm, setOpenConfirm] = React.useState(false);
  const [dialogText, setDialogText] = React.useState(<></>);
  const [resultAction, setResultAction] = React.useState(null);
  const [param, setParam] = React.useState(null);
  ///
  const Remove = (id, title) => {
    setOpenConfirm(true);
    setParam(id);
    setResultAction(() => ConfirmRemove);
    setDialogText(
      <>
        {getLocal("Remove Legal Document with title")} <strong>{title}</strong>.
      </>
    );
    const ConfirmRemove = (id) => {
      setOpenConfirm(false);
      deleteLegalProfile(id)
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
      headerName: getLocal("Title"),
      field: "title",
      flex: 0.3,
      renderCell: (params) => {
        const fileName = params.row.id
          .substr(params.row.id.length - 15)
          .replace("-", "");
        const userIdPath = params.row.userId
          .substr(params.row.userId.length - 15)
          .replace("-", "");
        const relativePath = `Users/LegalProfiles/${userIdPath}/${fileName}${params.row.fileExtension}`;
        return (
          <a
            href={`${process.env.NEXT_PUBLIC_API_URL}/${relativePath}`}
            target="_blank"
            rel="noreferrer"
          >
            {params.value}
          </a>
        );
      },
    },
    {
      headerName: getLocal("Document No"),
      field: "documentNumber",
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Type"),
      field: "legalDocumentTypeValue",
      flex: 0.25,
      renderCell: (params) => {
        const type = LegalDocumentTypes.find((v) => v.value == params.value);
        return type ? type.name : "";
      },
    },
    {
      headerName: getLocal("Expiry Date"),
      field: "expiryDateStr",
      flex: 0.2,
      renderCell: (params) => {
        return (
          <>
            {params.value}{" "}
            {params.row.isExpired ? (
              <>
                (<span className="text-danger">{getLocal("Expired")}</span>)
              </>
            ) : (
              ""
            )}
          </>
        );
      },
    },
    {
      headerName: getLocal("Status"),
      field: "status",
      flex: 0.1,
    },
    {
      headerName: getLocal("Actions"),
      field: "id",
      flex: 0.15,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          <Link href={`/myServices/legalProfileForm?id=${params.value}`} legacyBehavior>
            {getLocal("Edit")}
          </Link>
          {params.row.isLocked ? null : (
            <>
              &nbsp; &nbsp;
              <a
                className="text-danger"
                style={{ cursor: "pointer" }}
                onClick={() => Remove(params.value, params.row.title)}
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
            <FontAwesomeIcon icon={faFileContract} size="1x" />
          </div>

          <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
            <p className="links">
              {" "}
              {getLocal("Legal Documents")} (
              <Link href="/myServices/legalProfileForm" legacyBehavior>
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
              {<CustomDataGrid dataRows={list} columns={columns} />}
            </div>
            <div id="showOnSmallScreen">
              {list && list.length > 0 ? (
                list.map((r, i) => (
                  <Card key={i}>
                    <Card.Header>
                      <a
                        href={`${
                          process.env.NEXT_PUBLIC_API_URL
                        }/${`Users/LegalProfiles/${r.userId
                          .substr(r.userId.length - 15)
                          .replace("-", "")}/${r.id
                            .substr(r.id.length - 15)
                            .replace("-", "")}${r.fileExtension}`}`}
                        target="_blank"
                        rel="noreferrer"
                      >
                        {r.title}
                      </a>
                    </Card.Header>
                    <Card.Body>
                      <Row>
                        <Col>{getLocal("Document No")}</Col>
                        <Col>{r.documentNumber}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Type")}</Col>
                        <Col>
                          {LegalDocumentTypes.find(
                            (v) => v.value == r.legalDocumentTypeValue
                          )
                            ? LegalDocumentTypes.find(
                                (v) => v.value == r.legalDocumentTypeValue
                              ).name
                            : ""}
                        </Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Expiry Date")}</Col>
                        <Col>
                          <>
                            {r.expiryDateStr}{" "}
                            {r.isExpired ? (
                              <>
                                (
                                <span className="text-danger">
                                  {getLocal("Expired")}
                                </span>
                                )
                              </>
                            ) : (
                              ""
                            )}
                          </>
                        </Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Status")}</Col>
                        <Col>{r.status}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Actions")}</Col>
                        <Col>
                          <div>
                            <Link href={`/myServices/legalProfileForm?id=${r.id}`} legacyBehavior>
                              {getLocal("Edit")}
                            </Link>
                            {r.isLocked ? null : (
                              <>
                                &nbsp; &nbsp;
                                <a
                                  className="text-danger"
                                  style={{ cursor: "pointer" }}
                                  onClick={() => Remove(r.id, r.title)}
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
                <>No Legal Documents Found</>
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
