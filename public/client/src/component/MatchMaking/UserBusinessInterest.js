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
import { faThumbsUp } from "@fortawesome/free-solid-svg-icons";
import { AddAlert } from "@material-ui/icons";
import {
  getUserBusinessInterest,
  removeBusinessInterest,
  saveBusinessInterest,
} from "../../services/userBusinessInterest";
import UserBusinessInterestSectors from "./UserBusinessInterestSectors";
import { Button, Form } from "react-bootstrap";
import UserBusinessInterestProducts from "./UserBusinessInterestProducts";

export default function UserBusinessInterest({ expanded = true }) {
  const token = localStorage.getItem("access_token");
  const [userSectors, setUserSectors] = React.useState([]);
  const [details, setDetails] = React.useState([]);
  const [description, setDescription] = React.useState("");
  const [saving, setSaving] = React.useState(false);
  const [removing, setRemoving] = React.useState(false);
  React.useEffect(() => {
    loadUserInterest();
  }, []);

  const loadUserInterest = () => {
    getUserBusinessInterest().then((data) => {
      if (data.data) {
        if (data.data.description) {
          setDescription(data.data.description);
        } else {
          setDescription("");
        }
        if (data.data.sectors && data.data.sectors.$values) {
          setUserSectors(data.data.sectors.$values);
        } else {
          setUserSectors([]);
        }
        if (data.data.details && data.data.details.$values) {
          setDetails(data.data.details.$values);
        } else {
          setDetails([]);
        }
      } else {
      }
    });
  };
  //
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState("");
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
  const saveInterestDetail = () => {
    setSaving(true);
    const data = { description, sectors: userSectors, details };
    saveBusinessInterest(data).then((res) => {
      setSaving(false);
      if (res.data.saved) {
        loadUserInterest();
        setErrorMsg("Saved");
      } else {
        setErrorMsg(res.data.erorMsg);
      }
      setError(true);
      showNotification("tr");
    }).catch((error) => {
      setSaving(false);
      console.log(error);
      showNotification("tr");
    });;
  };
  //
  const removeInterest = () => {
    setRemoving(true);
    removeBusinessInterest().then((res) => {
      setRemoving(false);
      if (res.data.saved) {
        loadUserInterest();
        setErrorMsg("Removed");
      }
    });
  };
  return (
    <>
      <Grid item xs={12}>
        <Accordion defaultExpanded={expanded}>
          <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
            <div class="icon">
              <FontAwesomeIcon icon={faThumbsUp} />
            </div>
            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p class="links"> Business Interest</p>
            </div>
          </AccordionSummary>
          <AccordionDetails style={{ display: "block" }}>
            <Button
              variant="outline-danger"
              style={{ float: "right", margin: "0px", zIndex: "0" }}
              onClick={removeInterest}
              size="sm"
              disabled={saving || removing}
            >
              {removing ? "Removing ..." : "Remove"}
            </Button>
            <Button
              variant="outline-primary"
              style={{ float: "left", margin: "0px", zIndex: "0" }}
              onClick={saveInterestDetail}
              disabled={saving || removing}
            >
              {saving ? "Saving ..." : "Save"}
            </Button>
            <br />
            <br />
            <strong>Description</strong>
            <Form.Control
              as="textarea"
              rows="2"
              required={true}
              value={description}
              placeholder="Describe your Business Interest - የንግድ ፍላጎቶን ባጭሩ ይግለጹ"
              onChange={(e) => setDescription(e.target.value)}
            />
            <UserBusinessInterestSectors
              userSectors={userSectors}
              setUserSectors={setUserSectors}
              setError={setError}
              setErrorMsg={setErrorMsg}
              showNotification={showNotification}
            />

            <UserBusinessInterestProducts
              userProducts={details}
              setUserProducts={setDetails}
              setError={setError}
              setErrorMsg={setErrorMsg}
              showNotification={showNotification}
            />
          </AccordionDetails>
        </Accordion>
      </Grid>
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
