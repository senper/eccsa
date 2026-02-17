import React from "react";
import { Link } from "react-router-dom";

function EventTimeline({ selectedEvent }) {
  return (
    <div>
      <table className="table  table-striped">
        <thead>
          <tr>
            <th>Activity</th>
            <th>Start Date</th>
            <th>End Date</th>
          </tr>
        </thead>
        <tbody>
          {selectedEvent.detailsSorted.$values.map((detail) => {
            const value = selectedEvent.details.$values.find(
              (d) => d.$id == detail.$ref
            );
            let color = "text-danger";
            if (value.isFuture) {
              color = "text-primary";
            } else if (!value.isEnded) {
              color = "text-success";
            }
            const title =
              value.serviceTypeId && !value.isEnded && !value.isFuture ? (
                <Link
                className={color}
                  to={{
                    pathname: "/serviceRequestForm",
                    state: {
                      serviceTypeId: value.serviceTypeId,
                    },
                  }}
                >
                  {value.title}
                </Link>
              ) : (
                value.title
              );
            return (
              <tr>
                <td className={color}>{title}</td>
                <td className={color}>{value.startDateStr}</td>
                <td className={color}>{value.endDateStr}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
      {/* <Timeline align="alternate">
        {selectedEvent.sortedDetails.$values.map((value) => {
          let color = "text-danger";
          let dotColor = "secondary";
          let variant = "default";
          if (value.isFuthure) {
            color = "text-primary";
            dotColor = "primary";
            variant = "outlined";
          } else if (!value.isEnded && value.isEndDate) {
            color = "text-warning";
            dotColor = "secondary";
            variant = "outlined";
          } else if (!value.isEnded && !value.isEndDate) {
            color = "text-success";
            dotColor = "primary";
            variant = "outlined";
          }
          return (
            <>
              <TimelineItem>
                <TimelineOppositeContent>
                  <Typography className={color}>{value.dateStr}</Typography>
                </TimelineOppositeContent>
                <TimelineSeparator>
                  <TimelineDot variant={variant} color={dotColor} />
                  <TimelineConnector />
                </TimelineSeparator>
                <TimelineContent>
                  <Typography className={color}>{value.title}</Typography>
                </TimelineContent>
              </TimelineItem>
            </>
          );
        })}
      </Timeline> */}
    </div>
  );
}

export default EventTimeline;
