import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
  Snackbar,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { Button, Card, Col, Row } from "react-bootstrap";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import React from "react";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import { getLookupValues } from "../../pages/api/contactUs";
import {
  faListUl,
  faStickyNote,
  faTrash,
} from "@fortawesome/free-solid-svg-icons";
import { AddAlert } from "@mui/icons-material";
import RemarkDialog from "./RemarkDialog";
import ConfirmDialog from "../Common/ConfirmDialog";
import {
  deleteUserInterest,
  getSectorsList,
  getUserMatchSources,
  setInterestRemark,
} from "../../api/businessInterest";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import MMCompanyCol from "./MMCompanyCol";

export default function UserMatchesList({
  expanded = true,
  getLocal,
  getLocalByVal,
}) {
  const token = localStorage.getItem("access_token");
  const [businessTypes, setBusinessTypes] = React.useState([]);
  React.useEffect(() => {
    getLookupValues("00000000-0000-0000-0000-00000000002e").then((data) => {
      setBusinessTypes(data.data.$values);
    });
  }, []);
  //
  const [sectors, setSectors] = React.useState([]);
  React.useEffect(() => {
    getSectorsList().then((data) => {
      setSectors(data.data.sectors.$values);
    });
  }, []);
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
      loadMatches();
    }
  }, [token]);
  const loadMatches = () => {
    getUserMatchSources().then((data) => {
      setList(data.data.$values);
    });
  };
  //
  const [openConfirm, setOpenConfirm] = React.useState(false);
  const [dialogText, setDialogText] = React.useState(<></>);
  const [resultAction, setResultAction] = React.useState(null);
  const [param, setParam] = React.useState(null);
  ///
  const RemoveInterest = (id, name) => {
    setOpenConfirm(true);
    setParam(id);
    setResultAction(() => ConfirmRemoveInterest);
    setDialogText(
      <>
        Remove match with name <strong>{name}</strong>.
      </>
    );
    const ConfirmRemoveInterest = (id) => {
      setOpenConfirm(false);
      deleteUserInterest(id)
        .then((data) => {
          loadMatches();
        })
        .catch((error) => {
          // console.log(error);
          showNotification("tr");
        });
    };
  };
  //
  const [openRemarkDialog, setOpenRemarkDialog] = React.useState(false);
  const [remark, setRemark] = React.useState("");
  const [title, setTitle] = React.useState("");
  const [matchId, setMatchId] = React.useState("");
  const SetRemark = (id, name, remark) => {
    setRemark(remark);
    setTitle(name);
    setMatchId(id);
    setOpenRemarkDialog(true);
  };

  const SaveRemark = () => {
    setInterestRemark(matchId, remark)
      .then((data) => {
        loadMatches();
        setOpenRemarkDialog(false);
      })
      .catch((error) => {
        // console.log(error);
        showNotification("tr");
      });
  };
  //

  const [selectedSource, setSelectedSource] = React.useState(null);

  const columns = [
    {
      headerName: getLocal("Name"),
      field: "name",
      flex: 0.2,
      renderCell: (params) => (
        <a
          title="Go To Detail"
          className="text-danger"
          style={{ cursor: "pointer" }}
          onClick={() => {
            setSelectedSource(
              selectedSource && selectedSource.id == params.row.id
                ? null
                : params.row
            );
          }}
        >
          {params.value}
        </a>
      ),
    },
    {
      headerName: getLocal("Telephone"),
      field: "telephone",
      flex: 0.1,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Email"),
      field: "email",
      flex: 0.1,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Address"),
      field: "address",
      flex: 0.3,
      renderCell: (params) => {
        return renderCellExpand({ ...params, colDef: { width: "400px" } });
      },
    },
    {
      headerName: getLocal("Actions"),
      field: "id",
      flex: 0.09,
      filterable: false,
      sortable: false,
      renderCell: (params) => (
        <div className="actions-right">
          <a
            title={getLocal("Remark")}
            className="text-primary"
            style={{ cursor: "pointer" }}
            onClick={() =>
              SetRemark(params.id, params.row.name, params.row.remark)
            }
          >
            <FontAwesomeIcon icon={faStickyNote} />
          </a>
          &nbsp; &nbsp;
          <a
            title={getLocal("Remove")}
            className="text-danger"
            style={{ cursor: "pointer" }}
            onClick={() => RemoveInterest(params.id, params.row.name)}
          >
            <FontAwesomeIcon icon={faTrash} />
          </a>
        </div>
      ),
    },
  ];
  return (
    <>
      <Grid item xs={12}>
        <Accordion defaultExpanded={expanded}>
          <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
            <div className="icon">
              <FontAwesomeIcon icon={faListUl} size="1x" />
            </div>
            <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p className="links">
                {getLocal("Contacted Business Interests")}
              </p>
            </div>
          </AccordionSummary>
          <AccordionDetails style={{ display: "block" }}>
            {selectedSource && sectors && businessTypes ? (
              <>
                <Row>
                  <Col>
                    <Button
                      type="button"
                      style={{
                        margin: "0",
                        backgroundColor: "gray",
                        float: "left",
                      }}
                      variant="outline-secondary"
                      size="sm"
                      id="button-addon2"
                      onClick={() => setSelectedSource(null)}
                    >
                      {getLocal("Close Detail")}
                    </Button>
                  </Col>
                  <MMCompanyCol
                    sectors={sectors}
                    company={selectedSource}
                    businessTypes={businessTypes}
                    loadUserMatches={loadMatches}
                    userMatches={[{ sourceId: selectedSource.id }]}
                    expanded={true}
                    col={12}
                    showTitle={true}
                    getLocal={getLocal}
                  />
                </Row>
              </>
            ) : null}
            {
              <>
                <div id="showOnLargeScreen">
                  <CustomDataGrid dataRows={list} columns={columns} />
                </div>
                <div id="showOnSmallScreen">
                  {list && list.length > 0 ? (
                    list.map((r, i) => (
                      <Card key={i}>
                        <Card.Header>
                          <a
                            title="Go To Detail"
                            className="text-danger"
                            style={{ cursor: "pointer" }}
                            onClick={() => {
                              setSelectedSource(
                                selectedSource && selectedSource.id == r.id
                                  ? null
                                  : r
                              );
                            }}
                          >
                            {r.name}
                          </a>
                        </Card.Header>
                        <Card.Body>
                          <Row>
                            <Col>{getLocal("Telephone")}</Col>
                            <Col>{r.telephone}</Col>
                          </Row>
                          <Row>
                            <Col>{getLocal("Email")}</Col>
                            <Col>{r.email}</Col>
                          </Row>
                          <Row>
                            <Col>{getLocal("Address")}</Col>
                            <Col>{r.address}</Col>
                          </Row>
                          <Row>
                            <Col>{getLocal("Actions")}</Col>
                            <Col>
                              <div>
                                <a
                                  title={getLocal("Remark")}
                                  className="text-primary"
                                  style={{ cursor: "pointer" }}
                                  onClick={() =>
                                    SetRemark(r.id, r.name, r.remark)
                                  }
                                >
                                  <FontAwesomeIcon icon={faStickyNote} />
                                </a>
                                &nbsp; &nbsp;
                                <a
                                  title={getLocal("Remove")}
                                  className="text-danger"
                                  style={{ cursor: "pointer" }}
                                  onClick={() => RemoveInterest(r.id, r.name)}
                                >
                                  <FontAwesomeIcon icon={faTrash} />
                                </a>
                              </div>
                            </Col>
                          </Row>
                        </Card.Body>
                      </Card>
                    ))
                  ) : (
                    <>No Matches Found</>
                  )}
                </div>
              </>
            }
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
      />
      <RemarkDialog
        title={title}
        remark={remark}
        setRemark={setRemark}
        open={openRemarkDialog}
        setOpen={setOpenRemarkDialog}
        SaveRemark={SaveRemark}
        getLocal={getLocal}
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
  );
}
