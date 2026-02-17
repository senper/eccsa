import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import {
  Grid,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Snackbar,
} from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faListUl,
  faStickyNote,
  faTrash,
} from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import ConfirmDialog from "../Services/ConfirmDialog";
import { AddAlert } from "@material-ui/icons";
import {
  deleteUserInterest,
  getSectorsList,
  getUserMatchSources,
  setInterestRemark,
} from "../../services/businessInterest";
import { getLookupValues } from "../../services/contactUs";
import MMCompanyCol from "./MMCompanyCol";
import { Button, Col, Row } from "react-bootstrap";
import RemarkDialog from "./RemarkDialog";

export default function UserMatchesList({ expanded = true }) {
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
  }, []);
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
          console.log(error);
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
        console.log(error);
        showNotification("tr");
      });
  };
  //

  const [selectedSource, setSelectedSource] = React.useState(null);

  const columns = [
    {
      headerName: "Name",
      field: "name",
      flex: 0.2,
      renderCell: (params) => (
        <a
          title="Go To Detail"
          className="text-danger"
          href="javascript:void(0)"
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
      headerName: "Telephone",
      field: "telephone",
      flex: 0.1,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Email",
      field: "email",
      flex: 0.1,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Address",
      field: "address",
      flex: 0.3,
      renderCell: (params) => {
        return renderCellExpand({ ...params, colDef: { width: "400px" } });
      },
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.09,
      filterable: false,
      sortable: false,
      renderCell: (params) => (
        <div className="actions-right">
          <a
            title="Remark"
            className="text-primary"
            href="javascript:void(0)"
            onClick={() =>
              SetRemark(params.id, params.row.name, params.row.remark)
            }
          >
            <FontAwesomeIcon icon={faStickyNote} />
          </a>
          &nbsp; &nbsp;
          <a
            title="Remove"
            className="text-danger"
            href="javascript:void(0)"
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
            <div class="icon">
              <FontAwesomeIcon icon={faListUl} />
            </div>
            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p class="links">Contacted Business Interests</p>
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
                      Close Detail
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
                  />
                </Row>
              </>
            ) : null}
            {<CustomDataGrid dataRows={list} columns={columns} />}
          </AccordionDetails>
        </Accordion>
      </Grid>
      <ConfirmDialog
        open={openConfirm}
        setOpen={setOpenConfirm}
        btnValue={"Remove"}
        text={dialogText}
        resultAction={resultAction}
        param={param}
      />
      <RemarkDialog
        title={title}
        remark={remark}
        setRemark={setRemark}
        open={openRemarkDialog}
        setOpen={setOpenRemarkDialog}
        SaveRemark={SaveRemark}
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
  );
}
