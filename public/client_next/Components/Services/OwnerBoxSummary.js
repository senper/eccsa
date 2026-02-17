import { AccordionSummary } from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

export const OwnerBoxSummary = ({ so, getLocal }) => {
    return (
      <>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          style={{ marginBottom: "0" }}
        >
          <div className="icon">
            {/* <FontAwesomeIcon icon={faLandmark} /> */}
          </div>
  
          <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
            <p className="links" style={{ marginBottom: "0" }}>
              {" "}
              {so.title} {getLocal('Services')}
            </p>
          </div>
        </AccordionSummary>
      </>
    );
  };