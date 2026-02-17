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
  faAddressBook,
} from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import { getUserContacts, deleteUserContact } from "../../services/userContact";
import ConfirmDialog from "../Services/ConfirmDialog";
import { AddAlert } from "@material-ui/icons";

export default function UserContactsList({expanded = true}) {
  const token = localStorage.getItem("access_token");
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
      loadContacts();
    }
  }, []);
  const loadContacts = () => {
    getUserContacts().then((data) => {
      setList(data.data.$values);
    });
  };
  //
  const [openConfirm, setOpenConfirm] = React.useState(false);
  const [dialogText, setDialogText] = React.useState(<></>);
  const [resultAction, setResultAction] = React.useState(null);
  const [param, setParam] = React.useState(null);
  ///
  const RemoveContact = (id, type, address) => {
    setOpenConfirm(true);
    setParam(id);
    setResultAction(() => ConfirmRemoveContact);
    setDialogText(
      <>
        Remove contact with type <strong>{type}</strong> and address{" "}
        <strong>{address}</strong>.
      </>
    );
    const ConfirmRemoveContact = (id) => {
      setOpenConfirm(false);
      deleteUserContact(id)
        .then((data) => {
          loadContacts();
        })
        .catch((error) => {
          console.log(error);
          showNotification("tr");
        });
    };
  };

  const columns = [
    {
      headerName: "Priority",
      field: "priority",
      flex: 0.2,
    },
    {
      headerName: "Contact Type",
      field: "contactType",
      flex: 0.2,
    },
    {
      headerName: "Address",
      field: "address",
      flex: 0.3,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.09,
      filterable: false,
      sortable: false,
      renderCell: (params) => (
        <div className="actions-right">
          <Link
            to={{
              pathname: "/userContactForm",
              state: {
                id: params.value,
              },
            }}
          >
            {" "}
            Edit
          </Link>
          &nbsp; &nbsp;
          <a
            className="text-danger"
            href="javascript:void(0)"
            onClick={() =>
              RemoveContact(
                params.id,
                params.row.contactType,
                params.row.address
              )
            }
          >
            {" "}
            Remove
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
              <FontAwesomeIcon icon={faAddressBook} />
            </div>

            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p class="links">
                {" "}
                Contacts (<Link to="/userContactForm">Add New</Link>)
              </p>
            </div>
          </AccordionSummary>
          <AccordionDetails>
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
