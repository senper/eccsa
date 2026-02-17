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
import { faHandshake } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import {
  deleteExportAgreement,
  getExportAgreements,
} from "../../services/exportAgreement";
import ConfirmDialog from "../Services/ConfirmDialog";
import { AddAlert } from "@material-ui/icons";
import { Col, Form, InputGroup, Row } from "react-bootstrap";

export default function ExportAgreementsList({ expanded = true }) {
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
        Remove Export Agreement with agreement No <strong>{agreementNo}</strong>
        .
      </>
    );
    const ConfirmRemove = (id) => {
      setOpenConfirm(false);
      deleteExportAgreement(id)
        .then((data) => {
          loadList(filterStatus);
        })
        .catch((error) => {
          console.log(error);
          showNotification("tr");
        });
    };
  };
  const columns = [
    {
      headerName: "Agreement No",
      field: "agreementNo",
      flex: 0.2,
      renderCell: (params) => (
        <Link
          to={{
            pathname: "/exportAgreementDetail",
            state: {
              agreementId: params.row.id,
            },
          }}
        >
          {" "}
          {params.value}
        </Link>
      ),
    },
    {
      headerName: "License No",
      field: "licenseNo",
      flex: 0.2,
    },
    {
      headerName: "Consignee",
      field: "consignee",
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Country",
      field: "country",
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: "City",
      field: "city",
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Status",
      field: "status",
      flex: 0.15,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.15,
      filterable: false,
      sortable: false,
      renderCell: (params) => (
        <div className="actions-right">
          <Link
            to={{
              pathname: "/exportAgreementForm",
              state: {
                id: params.value,
              },
            }}
          >
            {" "}
            Edit
          </Link>
          {params.row.isLocked ? null : (
            <>
              &nbsp; &nbsp;
              <a
                className="text-danger"
                href="javascript:void(0)"
                onClick={() => Remove(params.id, params.row.agreementNo)}
              >
                {" "}
                Remove
              </a>
            </>
          )}
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
              <FontAwesomeIcon icon={faHandshake} />
            </div>

            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p class="links">
                {" "}
                Export Agreements (
                <Link to="/exportAgreementForm">Add New</Link>)
              </p>
            </div>
          </AccordionSummary>
          <AccordionDetails style={{ display: "block" }}>
            <Row>
              <Col md>
                <InputGroup>
                  <InputGroup.Text>Filter Status</InputGroup.Text>
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
