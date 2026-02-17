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
import { faFileContract } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import {
  deleteLegalProfile,
  getLegalProfiles,
  LegalDocumentTypes,
} from "../../services/legalProfile";
import ConfirmDialog from "../Services/ConfirmDialog";
import { AddAlert } from "@material-ui/icons";
import { Col, Form, InputGroup, Row } from "react-bootstrap";

export default function LegalProfilesList({ expanded = true }) {
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
        Remove Legal Document with title <strong>{title}</strong>.
      </>
    );
    const ConfirmRemove = (id) => {
      setOpenConfirm(false);
      deleteLegalProfile(id)
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
      headerName: "Title",
      field: "title",
      flex: 0.3,
      renderCell: (params) => {
        const fileName = params.row.id
          .substr(params.row.id.length - 15)
          .replace("-", "");
        const userIdPath = params.row.userId
          .substr(params.row.id.length - 15)
          .replace("-", "");
        const relativePath = `Users/LegalProfiles/${userIdPath}/${fileName}${params.row.fileExtension}`;
        return (
          <a
            href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
            target="_blank"
            rel="noreferrer"
          >
            {params.value}
          </a>
        );
      },
    },
    {
      headerName: "Document No",
      field: "documentNumber",
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Type",
      field: "legalDocumentTypeValue",
      flex: 0.25,
      renderCell: (params) => {
        const type = LegalDocumentTypes.find((v) => v.value == params.value);
        return type ? type.name : "";
      },
    },
    {
      headerName: "Expiry Date",
      field: "expiryDateStr",
      flex: 0.2,
      renderCell: (params) => {
        return (
          <>
            {params.value}{" "}
            {params.row.isExpired ? (
              <>
                (<span className="text-danger">Expired</span>)
              </>
            ) : (
              ""
            )}
          </>
        );
      },
    },
    {
      headerName: "Status",
      field: "status",
      flex: 0.1,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.15,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          <Link
            to={{
              pathname: "/legalProfileForm",
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
                onClick={() => Remove(params.id, params.row.title)}
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
              <FontAwesomeIcon icon={faFileContract} />
            </div>

            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p class="links">
                {" "}
                Legal Documents (<Link to="/legalProfileForm">Add New</Link>)
              </p>
            </div>
          </AccordionSummary>
          <AccordionDetails style={{ display: "block" }}>
            <Row >
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
