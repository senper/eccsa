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
import { faAddressBook } from "@fortawesome/free-solid-svg-icons";
import Link from "next/link";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import {
  getUserContacts,
  deleteUserContact,
} from "../../pages/api/userContact";
import ConfirmDialog from "../Common/ConfirmDialog";
import { AddAlert } from "@mui/icons-material";
import { Card, Col, Row } from "react-bootstrap";

export default function UserContactsList({
  expanded = true,
  getLocal,
  getLocalByVal,
}) {
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
    loadContacts();
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
        {getLocal("Remove contact with type")} <strong>{type}</strong> and
        address <strong>{address}</strong>.
      </>
    );
    const ConfirmRemoveContact = (id) => {
      setOpenConfirm(false);
      deleteUserContact(id)
        .then((data) => {
          loadContacts();
        })
        .catch((error) => {
          // console.log(error);
          showNotification("tr");
        });
    };
  };

  const columns = [
    {
      headerName: getLocal("Priority"),
      field: "priority",
      flex: 0.2,
    },
    {
      headerName: getLocal("Contact Type"),
      field: "contactType",
      flex: 0.2,
    },
    {
      headerName: getLocal("Address"),
      field: "address",
      flex: 0.3,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Actions"),
      field: "id",
      flex: 0.09,
      filterable: false,
      sortable: false,
      renderCell: (params) => (
        <div className="actions-right">
          <Link href={`/myServices/userContactForm?id=${params.value}`} legacyBehavior>
            {getLocal("Edit")}
          </Link>
          &nbsp; &nbsp;
          <a
            className="text-danger"
            style={{ cursor: "pointer" }}
            onClick={() =>
              RemoveContact(
                params.value,
                params.row.contactType,
                params.row.address
              )
            }
          >
            {getLocal("Remove")}
          </a>
        </div>
      ),
    },
  ];
  return <>
    <Grid item xs={12}>
      <Accordion defaultExpanded={expanded}>
        <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
          <div className="icon">
            <FontAwesomeIcon icon={faAddressBook} size="1x" />
          </div>

          <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
            <p className="links">
              {" "}
              {getLocal("Contacts")} (
              <Link href="/myServices/userContactForm" legacyBehavior>
                {getLocal("Add New")}
              </Link>
              )
            </p>
          </div>
        </AccordionSummary>
        <AccordionDetails>
            <>
            <div id="showOnLargeScreen">
              <CustomDataGrid dataRows={list} columns={columns} />
            </div>
            <div id="showOnSmallScreen">
              {list && list.length > 0 ? (
                list.map((r, i) => (
                  <Card key={i}>
                    <Card.Header>
                      {r.address}-{r.requestDateStr}
                    </Card.Header>
                    <Card.Body>
                      <Row>
                        <Col>{getLocal("Priority")}</Col>
                        <Col>{r.priority}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Contact Type")}</Col>
                        <Col>{r.contactType}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Address")}</Col>
                        <Col>{r.address}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Actions")}</Col>
                        <Col>
                          <div>
                            <Link href={`/myServices/userContactForm?id=${r.id}`} legacyBehavior>
                              {getLocal("Edit")}
                            </Link>
                            &nbsp; &nbsp;
                            <a
                              className="text-danger"
                              style={{ cursor: "pointer" }}
                              onClick={() =>
                                RemoveContact(r.id, r.contactType, r.address)
                              }
                            >
                              {getLocal("Remove")}
                            </a>
                          </div>
                        </Col>
                      </Row>
                    </Card.Body>
                  </Card>
                ))
              ) : (
                <>No Contact(s) Found</>
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
