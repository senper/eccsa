import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import {
  Grid,
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLandmark, faPoll } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import { getSurveyList } from "../../services/survey";

export default function SurveyList({expanded = true}) {
  const token = localStorage.getItem("access_token");
  const [surveys, setSurveys] = React.useState([]);
  React.useEffect(() => {
    if (token) {
      getSurveyList().then((data) => {
        setSurveys(data.data.$values);
      });
    }
  }, []);

  const columns = [
    {
      headerName: "Title",
      field: "title",
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Description",
      field: "description",
      flex: 0.25,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Open Days",
      field: "setting",
      flex: 0.1,
      valueGetter: (params) => {
        return params.value.openDays;
      },
    },
    {
      headerName: "Status",
      field: "setting",
      valueGetter: (params) => {
        const questionsCount = params.value.questionsCount;
        const responsesCount = params.value.responsesCount;
        return `${responsesCount} of ${questionsCount}`;
      },
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.09,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          <Link
            to={{
              pathname: "/surveyQuestionForm",
              state: {
                surveyId: params.value,
              },
            }}
          >
            {" "}
            Detail
          </Link>
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
              <FontAwesomeIcon icon={faPoll} />
            </div>
            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p class="links"> Surveys</p>
            </div>
          </AccordionSummary>
          <AccordionDetails>
            {<CustomDataGrid dataRows={surveys} columns={columns} />}
          </AccordionDetails>
        </Accordion>
      </Grid>
    </>
  );
}
